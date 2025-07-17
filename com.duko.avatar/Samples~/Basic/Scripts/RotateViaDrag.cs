using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // RotateViaDrag
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:49
    //******************************************
    public class RotateViaDrag : MonoBehaviour
    {
        public Transform RotateTarget;
        public float DragIntensity = 1;
        private float _Rotate = 180;

        private bool _Dragging;
        private Vector3 _LastPosition;
        public Vector3 DeltaPosition => Input.mousePosition - _LastPosition;
        
        private void Update()
        {
            if (_CheckDragging())
            {
                _Rotate += DeltaPosition.x * DragIntensity;
            }

            if (RotateTarget == null)
            {
                return;
            }
            RotateTarget.rotation = Quaternion.Euler(0, _Rotate, 0);   
        }
        
        private bool _CheckDragging()
        {
            if (!_Dragging && Input.GetMouseButtonDown(0))
            {
                _Dragging = true;
            }
            if (_Dragging && Input.GetMouseButtonUp(0))
            {
                _Dragging = false;
            }
            return _Dragging;
        }


        private void LateUpdate()
        {
            _LastPosition = Input.mousePosition;
        }
    }
}