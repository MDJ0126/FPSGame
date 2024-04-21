using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PartsRendererManager : MonoBehaviour
{
    private static Dictionary<eParts, List<ePartsResources>> _resourceGroup;

    static PartsRendererManager()
    {
        _resourceGroup = new();
        var enumParts = Enum.GetValues(typeof(eParts));
        foreach (var value in enumParts)
        {
            eParts part = (eParts)value;
            _resourceGroup.Add(part, new());
        }

        var enumPartsResources = Enum.GetValues(typeof(ePartsResources));
        foreach (var value in enumPartsResources)
        {
            ePartsResources partsResource = (ePartsResources)value;
            string path = partsResource.ToDescription();
            string[] partsName = path.Split('/');
            string parentName = partsName[2];

            eParts parts = eParts.None;
            foreach (var value2 in enumParts)
            {
                eParts parts2 = (eParts)value2;
                if (parts2.ToString() == parentName)
                {
                    parts = parts2;
                    break;
                }
            }
            if (_resourceGroup.TryGetValue(parts, out var list))
            {
                list.Add(partsResource);
            }
        }
    }

    #region Inspector

    /// <summary>
    /// 모자
    /// </summary>
    public Transform hat;
    /// <summary>
    /// 헤어
    /// </summary>
    public Transform hair;
    /// <summary>
    /// 마스크
    /// </summary>
    public Transform facemask;
    /// <summary>
    /// 얼굴 장식
    /// </summary>
    public Transform faceAcc;
    /// <summary>
    /// 수염
    /// </summary>
    public Transform beard;
    /// <summary>
    /// 스카프
    /// </summary>
    public Transform scarf;
    /// <summary>
    /// 가방
    /// </summary>
    public Transform bag;
    /// <summary>
    /// 패치
    /// </summary>
    public Transform patch;
    /// <summary>
    /// 파우치
    /// </summary>
    public Transform pouch;

    #endregion

    private Dictionary<eParts, Transform> _partsPos = new();
    private Dictionary<eParts, GameObject> _equippedPartsList = new();

    private void Awake()
    {
        _partsPos.Add(eParts.Hat, hat);
        _partsPos.Add(eParts.Hair, hair);
        _partsPos.Add(eParts.Facemask, facemask);
        _partsPos.Add(eParts.FaceAcc, faceAcc);
        _partsPos.Add(eParts.Beard, beard);
        _partsPos.Add(eParts.Scarf, scarf);
        _partsPos.Add(eParts.Bag, bag);
        _partsPos.Add(eParts.Patch, patch);
        _partsPos.Add(eParts.Pouch, pouch);
    }

    private void Start()
    {
        if (Utils.IsRandom(0.5f)) EquipRandomParts(eParts.Hat);
        if (Utils.IsRandom(1f)) EquipRandomParts(eParts.Hair);
        if (Utils.IsRandom(0.2f)) EquipRandomParts(eParts.Facemask);
        if (Utils.IsRandom(0.2f)) EquipRandomParts(eParts.FaceAcc);
        if (Utils.IsRandom(0.2f)) EquipRandomParts(eParts.Beard);
        if (Utils.IsRandom(0.5f)) EquipRandomParts(eParts.Scarf);
        if (Utils.IsRandom(1f)) EquipRandomParts(eParts.Bag);
        if (Utils.IsRandom(0.5f)) EquipRandomParts(eParts.Patch);
        if (Utils.IsRandom(0.5f)) EquipRandomParts(eParts.Pouch);
    }

    /// <summary>
    /// 파츠 장착
    /// </summary>
    /// <param name="parts">부위</param>
    /// <param name="partsResources">파츠 리소스 주소</param>
    public void EquipParts(eParts parts, ePartsResources partsResources)
    {
        if (_partsPos.TryGetValue(parts, out Transform ts))
        {
            if (ts)
            {
                // 기존 장착된 파츠 제거
                if (_equippedPartsList.TryGetValue(parts, out GameObject equippedParts))
                {
                    if (equippedParts)
                    {
                        Destroy(equippedParts);
                    }
                    _equippedPartsList.Remove(parts);
                }

                // 새로운 파츠 장착
                string resourcePath = partsResources.ToDescription();
                var go = Instantiate(Resources.Load(resourcePath)) as GameObject;
                go.transform.SetParent(ts);
                go.transform.Initialize();
                _equippedPartsList.Add(parts, go);
            }
        }
    }

    /// <summary>
    /// 랜덤 파츠 장착
    /// </summary>
    /// <param name="parts"></param>
    public void EquipRandomParts(eParts parts)
    {
        if (_resourceGroup.TryGetValue(parts, out var list))
        {
            EquipParts(parts, list[UnityEngine.Random.Range(0, list.Count)]);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Setting")]
    private void AutoSetting()
    {
        var childs = this.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < childs.Length; i++)
        {
            Transform child = childs[i];
            switch (child.name)
            {
                case "Head":
                    {
                        hat = child;
                        hair = child;
                        faceAcc = child;
                        facemask = child;
                        beard = child;
                        scarf = child;
                    }
                    break;
                case "Spine_03":
                    {
                        bag = child;
                    }
                    break;
            }
        }
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}