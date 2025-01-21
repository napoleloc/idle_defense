using EncosyTower.Modules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using Unity.Collections;
using UnityEngine;
using EncosyTower.Modules.Collections;
using Module.GameUI.Talents.Control;
using EncosyTower.Modules.Collections.Unsafe;
using Module.Core;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Module.GameUI.Talents
{
    using UnityObject = UnityEngine.Object;

    public class TalentPool : MonoBehaviour
    {
        private readonly List<UnityObject> _objectList = new();
        private readonly FasterList<TalentControl> _unusedAttributeControls = new();
        private readonly FasterList<int> _unusedInstanceIds = new();
        private readonly FasterList<int> _unusedTransformIds = new();

        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private GameObject _prefab;

        public Scene PoolScene { get; private set; }

        public int UnusedCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _unusedAttributeControls.Count;
        }

        public void Initialize(int prepoolAmount)
        {
            PoolScene = gameObject.scene;
            
            Prepool(prepoolAmount, PoolScene);
        }

        public void Deinitialize()
        {
            
        }

        private void Prepool(int amount, Scene poolScene)
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
                PrepoolMany(amount, poolScene);
            }

            void PrepoolOne()
            {
                var go = InstantiateInternal(poolScene);
                var component = go.GetOrAddComponent<TalentControl>();

                if (go.IsInvalid())
                {
                    return;
                }

                _unusedAttributeControls.IncreaseCapacityBy(1);
                _unusedInstanceIds.IncreaseCapacityBy(1);
                _unusedTransformIds.IncreaseCapacityBy(1);

                _unusedAttributeControls.Add(component);
                _unusedInstanceIds.Add(go.GetInstanceID());
                _unusedTransformIds.Add(go.transform.GetInstanceID());

                go.SetActive(false);
            }
        }

        [Button(buttonSize: 35)]
        private void PrepoolMany(int amount, Scene poolScene)
        {
            InstantiateInternal(amount, Allocator.Temp, poolScene, out var instanceIds, out var transformIds);

            _objectList.Clear();
            _objectList.Capacity = Mathf.Max(_objectList.Capacity, amount);

            Resources.InstanceIDToObjectList(instanceIds, _objectList);

            var unusedAttributeControls = _unusedAttributeControls;
            unusedAttributeControls.IncreaseCapacityBy(amount);
            _unusedInstanceIds.IncreaseCapacityBy(amount);
            _unusedTransformIds.IncreaseCapacityBy(amount);

            var objects = _objectList.AsReadOnlySpanUnsafe();
            var objectsLength = objects.Length;
            var trimCloneSuffix = false;

            for (var i = 0; i < objectsLength; i++)
            {
                var go = objects[i] as GameObject;
                var talenControl = go.GetOrAddComponent<TalentControl>();

                if (talenControl.IsInvalid())
                {
                    continue;
                }

                if (trimCloneSuffix)
                {
                    talenControl.TrimCloneSuffix();
                }

                unusedAttributeControls.Add(talenControl);
            }

            _unusedInstanceIds.AddRange(instanceIds.AsReadOnlySpan());
            _unusedTransformIds.AddRange(transformIds.AsReadOnlySpan());

            _objectList.Clear();

            GameObject.SetGameObjectsActive(instanceIds, false);
        }

        public TalentControl Rent(bool activate = true)
        {
            if(UnusedCount < 1)
            {
                Prepool(1, PoolScene);
            }

            var last = UnusedCount - 1;
            var result = _unusedAttributeControls[last];

            _unusedAttributeControls.RemoveAt(last);
            _unusedInstanceIds.RemoveAt(last);
            _unusedTransformIds.RemoveAt(last);

            if (activate)
            {
                result.gameObject.SetActive(true);
            }

            return result;
        }

        public void Rent(Span<TalentControl> instances, bool activate = true)
        {
            var length = instances.Length;

            Assert.IsTrue(length > 0, "array does not have enough space to contain the items");

            Prepool(length - UnusedCount, PoolScene);

            var startIndex = UnusedCount - length;
            var instanceIds = NativeArray.CreateFast<int>(length, Allocator.Temp);

            _unusedAttributeControls.CopyTo(startIndex, instances);
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);

            _unusedAttributeControls.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void Rent(Span<TalentControl> instances, Span<int> transformIds, bool activate = true)
        {
            var length = instances.Length;

            Assert.IsTrue(length == transformIds.Length, "arrays do not have the same size");
            Assert.IsTrue(length > 0, "arrays do not have enough space to contain the items");

            Prepool(length - UnusedCount, PoolScene);

            var startIndex = UnusedCount - length;
            var instanceIds = NativeArray.CreateFast<int>(length, Allocator.Temp);

            _unusedAttributeControls.CopyTo(startIndex, instances);
            _unusedInstanceIds.CopyTo(startIndex, instanceIds);
            _unusedTransformIds.CopyTo(startIndex, transformIds);

            _unusedAttributeControls.RemoveAt(startIndex, length);
            _unusedInstanceIds.RemoveAt(startIndex, length);
            _unusedTransformIds.RemoveAt(startIndex, length);

            //Prefab.MoveToScene(instanceIds);

            if (activate)
            {
                GameObject.SetGameObjectsActive(instanceIds, true);
            }
        }

        public void Return(TalentControl instance)
        {
            if (instance.IsInvalid())
            {
                return;
            }

            instance = instance.AlwaysValid();

            _unusedAttributeControls.Add(instance);
            _unusedInstanceIds.Add(instance.gameObject.GetInstanceID());
            _unusedTransformIds.Add(instance.transform.GetInstanceID());

            instance.gameObject.SetActive(false);
        }

        [Button(buttonSize:35)]
        public void ReleaseInstances(int keep, Action<TalentControl> onReleased = null)
        {
            keep = Mathf.Max(keep, 0);

            var removeCount = UnusedCount - keep;

            if (removeCount < 1)
            {
                return;
            }

            var objectSpan = _unusedAttributeControls.AsSpan()[keep..];

            for (var i = objectSpan.Length - 1; i >= 0; i--)
            {
                var component = objectSpan[i];
                onReleased?.Invoke(component);
                UnityObject.Destroy(component);
            }

            _unusedAttributeControls.RemoveAt(keep, removeCount);
            _unusedInstanceIds.RemoveAt(keep, removeCount);
            _unusedTransformIds.RemoveAt(keep, removeCount);
        }



        private void InstantiateInternal(
            int count
            , Allocator allocator
            , Scene poolScene
            , out NativeArray<int> instanceIds
            , out NativeArray<int> transformIds)
        {
            if (count <= 0)
            {
                instanceIds = default;
                transformIds = default;
                return;
            }

            if (_prefab.IsInvalid())
            {
                throw new NullReferenceException(nameof(_prefab));
            }

            instanceIds = NativeArray.CreateFast<int>(count, allocator);
            transformIds = NativeArray.CreateFast<int>(count, allocator);

            GameObject.InstantiateGameObjects(
                          _prefab.GetInstanceID()
                        , count
                        , instanceIds
                        , transformIds
                        , poolScene
                    );

            if (_parent.IsValid())
            {
                var list = new List<UnityEngine.Object>(transformIds.Length);

                Resources.InstanceIDToObjectList(transformIds, list);

                var span = list.AsReadOnlySpanUnsafe();
                var spanLength = span.Length;
                var inWorldSpace = false;

                for (var i = 0; i < spanLength; i++)
                {
                    var transform = span[i] as Transform;

                    if (transform.IsInvalid())
                    {
                        continue;
                    }

                    transform.SetParent(_parent, inWorldSpace);
                }
            }
        }

        private GameObject InstantiateInternal(Scene poolScene)
        {
            var result = UnityEngine.Object.Instantiate(_prefab);
            SceneManager.MoveGameObjectToScene(result, poolScene);
            return result;
        }
    }
}
