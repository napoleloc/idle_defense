//MIT License
//
//Copyright (c) 2024 Laicasaane
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using EncosyTower.Modules;
using EncosyTower.Modules.Collections;
using EncosyTower.Modules.Collections.Unsafe;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Module.Core.Pooling
{
    using Location = ComponentPrefab.Location;
    using UnityObject = UnityEngine.Object;

    public class ComponentPool<TPrefab, TComponent>
        where TPrefab : ComponentPrefab
        where TComponent : Component
    {
        private readonly ComponentPrefab _prefab;
        private readonly FasterList<TComponent> _unusedComponents;
        private readonly FasterList<int> _unusedInstanceIds;
        private readonly FasterList<int> _unusedTransformIds;
        private readonly List<UnityObject> _objectList;

        public ComponentPool([NotNull] ComponentPrefab prefab)
        {
            _prefab = prefab;
            _unusedComponents = new FasterList<TComponent>();
            _unusedInstanceIds = new FasterList<int>();
            _unusedTransformIds = new FasterList<int>();
            _objectList = new List<UnityObject>();
        }

        public int UnusedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _unusedComponents.Count;
        }

        public ComponentPrefab Prefab
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _prefab;
        }

        public bool TrimCloneSuffix { get; set; }

        public void Prepool(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            if (amount <= 1)
            {
                PrepoolOne();
            }
            else
            {
                PrepoolMany(amount);
            }

            void PrepoolOne()
            {
                var go = _prefab.Instantiate(Location.Parent, Location.PoolScene, Location.Scene);
                var component = go.GetComponent<TComponent>();

                if (go.IsInvalid())
                {
                    return;
                }

                if (TrimCloneSuffix)
                {
                    go.TrimCloneSuffix();
                }

                _unusedComponents.IncreaseCapacityBy(1);
                _unusedInstanceIds.IncreaseCapacityBy(1);
                _unusedTransformIds.IncreaseCapacityBy(1);

                _unusedComponents.Add(component);
                _unusedInstanceIds.Add(go.GetInstanceID());
                _unusedTransformIds.Add(go.transform.GetInstanceID());

                go.SetActive(false);
            }
        }

        private void PrepoolMany(int amount)
        {
            if (_prefab.Instantiate(amount
                , Allocator.Temp
                , out var instanceIds
                , out var transformIds
                , Location.Parent, Location.PoolScene, Location.Scene
            ) == false)
            {
                return;
            }

            _objectList.Clear();
            _objectList.Capacity = Mathf.Max(_objectList.Capacity, amount);

            Resources.InstanceIDToObjectList(instanceIds, _objectList);

            var unusedComponents = _unusedComponents;
            unusedComponents.IncreaseCapacityBy(amount);
            _unusedInstanceIds.IncreaseCapacityBy(amount);
            _unusedTransformIds.IncreaseCapacityBy(amount);

            var objects = _objectList.AsReadOnlySpanUnsafe();
            var objectsLength = objects.Length;
            var trimCloneSuffix = TrimCloneSuffix;

            for (var i = 0; i < objectsLength; i++)
            {
                var go = objects[i] as GameObject;
                var component = go.GetComponent<TComponent>();

                if (go.IsInvalid())
                {
                    continue;
                }

                if (trimCloneSuffix)
                {
                    go.TrimCloneSuffix();
                }

                unusedComponents.Add(component);
            }

            _unusedInstanceIds.AddRange(instanceIds.AsReadOnlySpan());
            _unusedTransformIds.AddRange(transformIds.AsReadOnlySpan());

            _objectList.Clear();

            GameObject.SetGameObjectsActive(instanceIds, false);
        }

        public void ReleaseInstances(int keep, Action<TComponent> onReleased = null)
        {
            keep = Mathf.Max(keep, 0);

            var removeCount = UnusedCount - keep;

            if (removeCount < 1)
            {
                return;
            }

            var componentSpan = _unusedComponents.AsSpan()[keep..];

            for (var i = componentSpan.Length - 1; i >= 0; i--)
            {
                var component = componentSpan[i];
                onReleased?.Invoke(component);
                UnityObject.Destroy(component);
            }

            _unusedComponents.RemoveAt(keep, removeCount);
            _unusedInstanceIds.RemoveAt(keep, removeCount);
            _unusedTransformIds.RemoveAt(keep, removeCount);
        }

        public TComponent RentGameObject(bool activate = false)
        {
            if (UnusedCount < 1)
            {
                Prepool(1);
            }

            var last = UnusedCount - 1;
            var result = _unusedComponents[last];

            _unusedComponents.RemoveAt(last);
            _unusedInstanceIds.RemoveAt(last);
            _unusedTransformIds.RemoveAt(last);

            Prefab.MoveToScene(result.gameObject);

            if (activate)
            {
                result.gameObject.SetActive(true);
            }

            return result;
        }

        public int RentInstanceId(bool activate = false)
        {
            if (UnusedCount < 1)
            {
                Prepool(1);
            }

            var last = UnusedCount - 1;
            var component = _unusedComponents[last];
            var result = _unusedInstanceIds[last];

            _unusedComponents.RemoveAt(last);
            _unusedInstanceIds.RemoveAt(last);
            _unusedTransformIds.RemoveAt(last);

            Prefab.MoveToScene(component.gameObject);

            if (activate)
            {
                component.gameObject.SetActive(true);
            }

            return result;
        }

        public int RentTransformId(bool activate = false)
        {
            if (UnusedCount < 1)
            {
                Prepool(1);
            }

            var last = UnusedCount - 1;
            var obj = _unusedComponents[last].gameObject;
            var result = _unusedTransformIds[last];

            _unusedComponents.RemoveAt(last);
            _unusedInstanceIds.RemoveAt(last);
            _unusedTransformIds.RemoveAt(last);

            Prefab.MoveToScene(obj);

            if (activate)
            {
                obj.SetActive(true);
            }

            return result;
        }

        public void Rent(Span<int> instanceIds, Span<int> transformIds, bool activate = false)
        {
            var length = instanceIds.Length;

            Assert.IsTrue(length == transformIds.Length, "arrays do not have the same size");
            Assert.IsTrue(length > 0, "arrays do not have enough space to contain the items");

            Prepool(length - UnusedCount);

            var startIndex = UnusedCount - length;
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);
            _unusedTransformIds.CopyTo(startIndex, transformIds);

            _unusedComponents.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void Rent(
              Span<TComponent> components
            , Span<int> instanceIds
            , Span<int> transformIds
            , bool activate = false
        )
        {
            var length = components.Length;

            Assert.IsTrue(length == instanceIds.Length && length == transformIds.Length, "arrays do not have the same size");
            Assert.IsTrue(length > 0, "arrays do not have enough space to contain the items");

            Prepool(length - UnusedCount);

            var startIndex = UnusedCount - length;
            _unusedComponents.CopyTo(startIndex, components);
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);
            _unusedTransformIds.CopyTo(startIndex, transformIds);

            _unusedComponents.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void RentComponents(Span<TComponent> components, bool activate = false)
        {
            var length = components.Length;

            Assert.IsTrue(length > 0, "\"objects\" array does not have enough space to contain the items");

            Prepool(length - UnusedCount);

            var startIndex = UnusedCount - length;
            var instanceIds = NativeArray.CreateFast<int>(length, Allocator.Temp);

            _unusedComponents.CopyTo(startIndex, components);
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);

            _unusedComponents.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void RentInstanceIds(Span<int> instanceIds, bool activate = false)
        {
            var length = instanceIds.Length;

            Assert.IsTrue(length > 0, "\"instanceIds\" array does not have enough space to contain the items");

            Prepool(length - UnusedCount);

            var startIndex = UnusedCount - length;
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);

            _unusedComponents.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void RentTransformIds(Span<int> transformIds, bool activate = false)
        {
            var length = transformIds.Length;

            Assert.IsTrue(length > 0, "\"transformIds\" array does not have enough space to contain the items");

            Prepool(length - UnusedCount);

            var startIndex = UnusedCount - length;
            var instanceIds = NativeArray.CreateFast<int>(length, Allocator.Temp);

            _unusedTransformIds.CopyTo(startIndex, transformIds);
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);

            _unusedComponents.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void Return(TComponent instance)
        {
            if (instance.IsInvalid())
            {
                return;
            }

            instance = instance.AlwaysValid();

            Prefab.MoveToScene(instance.gameObject, true);

            _unusedComponents.Add(instance);
            _unusedInstanceIds.Add(instance.GetInstanceID());
            _unusedTransformIds.Add(instance.transform.GetInstanceID());

            instance.gameObject.SetActive(false);
        }

        public void ReturnComponents(ReadOnlySpan<TComponent> components)
        {
            var length = components.Length;

            if (length < 1)
            {
                return;
            }

            var unusedComponents = _unusedComponents;
            var unusedInstanceIds = _unusedInstanceIds;
            var unusedTransformIds = _unusedTransformIds;
            var capacity = unusedComponents.Count + length;

            unusedComponents.IncreaseCapacityTo(capacity);
            unusedInstanceIds.IncreaseCapacityTo(capacity);
            unusedTransformIds.IncreaseCapacityTo(capacity);

            var postIds = NativeArray.CreateFast<int>(length, Allocator.Temp);
            var postIdsLength = 0;

            for (var i = 0; i < length; i++)
            {
                var component = components[i];

                if (component.IsInvalid())
                {
                    continue;
                }

                unusedComponents.Add(component);
                unusedInstanceIds.Add(postIds[postIdsLength++] = component.gameObject.GetInstanceID());
                unusedTransformIds.Add(component.transform.GetInstanceID());
            }

            postIds = postIds.GetSubArray(0, postIdsLength);
            Prefab.MoveToScene(postIds, true);
            GameObject.SetGameObjectsActive(postIds, false);
        }

        public void ReturnInstanceIds(NativeArray<int> instanceIds)
        {
            var length = instanceIds.Length;

            if (length < 1)
            {
                return;
            }

            _objectList.Clear();
            _objectList.Capacity = Mathf.Max(_objectList.Capacity, length);

            Resources.InstanceIDToObjectList(instanceIds, _objectList);

            var unusedObjects = _unusedComponents;
            var unusedInstanceIds = _unusedInstanceIds;
            var unusedTransformIds = _unusedTransformIds;
            var capacity = unusedObjects.Count + length;

            unusedObjects.IncreaseCapacityTo(capacity);
            unusedInstanceIds.IncreaseCapacityTo(capacity);
            unusedTransformIds.IncreaseCapacityTo(capacity);

            var objects = _objectList.AsReadOnlySpanUnsafe();
            var postIds = NativeArray.CreateFast<int>(length, Allocator.Temp);
            var postIdsLength = 0;

            for (var i = 0; i < length; i++)
            {
                var obj = objects[i] as GameObject;
                var component = obj.GetComponent<TComponent>();

                if (obj.IsInvalid())
                {
                    continue;
                }

                unusedObjects.Add(component);
                unusedInstanceIds.Add(postIds[postIdsLength++] = instanceIds[i]);
                unusedTransformIds.Add(obj.transform.GetInstanceID());
            }

            _objectList.Clear();

            postIds = postIds.GetSubArray(0, postIdsLength);
            Prefab.MoveToScene(postIds, true);
            GameObject.SetGameObjectsActive(postIds, false);
        }

        public void ReturnTransformIds(NativeArray<int> transformIds)
        {
            var length = transformIds.Length;

            if (length < 1)
            {
                return;
            }

            _objectList.Clear();
            _objectList.Capacity = Mathf.Max(_objectList.Capacity, length);

            Resources.InstanceIDToObjectList(transformIds, _objectList);

            var unusedObjects = _unusedComponents;
            var unusedInstanceIds = _unusedInstanceIds;
            var unusedTransformIds = _unusedTransformIds;
            var capacity = unusedObjects.Count + length;

            unusedObjects.IncreaseCapacityTo(capacity);
            unusedInstanceIds.IncreaseCapacityTo(capacity);
            unusedTransformIds.IncreaseCapacityTo(capacity);

            var objects = _objectList.AsReadOnlySpanUnsafe();
            var postIds = NativeArray.CreateFast<int>(length, Allocator.Temp);
            var postIdsLength = 0;

            for (var i = 0; i < length; i++)
            {
                var transform = objects[i] as Transform;

                if (transform.IsInvalid())
                {
                    continue;
                }

                var component = transform.GetComponent<TComponent>();

                if (component.IsInvalid())
                {
                    continue;
                }

                unusedObjects.Add(component);
                unusedInstanceIds.Add(postIds[postIdsLength++] = component.gameObject.GetInstanceID());
                unusedTransformIds.Add(transformIds[i]);
            }

            _objectList.Clear();

            postIds = postIds.GetSubArray(0, postIdsLength);
            Prefab.MoveToScene(postIds, true);
            GameObject.SetGameObjectsActive(postIds, false);
        }
    }
}
