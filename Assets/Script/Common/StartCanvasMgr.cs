using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ClashGame
{
    /// <summary>
    /// 游戏中Canvas管理器
    /// </summary>
    public class StartCanvasMgr : MonoBehaviour
    {
        private static GameObject uiRoot;
        private float KuanGaoBi = 1.5f; //960/640
        //Canvas层级关系，反序*100对应Canvas的Plane Distance

        void Awake()
        {
            float SheBeiKuanGaoBi = Screen.width / (float)Screen.height;
            if(SheBeiKuanGaoBi < KuanGaoBi)
            {
                transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
            }else
            {
                transform.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
            }
        }


    }

}
