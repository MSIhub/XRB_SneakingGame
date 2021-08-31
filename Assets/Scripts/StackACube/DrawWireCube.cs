using UnityEngine;

namespace StackACube
{
    public class DrawWireCube : MonoBehaviour
    {
        [SerializeField] private Transform _cube;

        [SerializeField] private LineRenderer _line;
        // Start is called before the first frame update
        void Start()
        {
            _line.positionCount = 26;
        }

        // Update is called once per frame
        void Update()
        {
            WireCube();
        }

        private void WireCube()//assuming rotation is zero
        {
            var localScale = _cube.localScale*0.5f;
            
            var position =_cube.position;
            //Top Conners
            Vector3 p1 = position + new Vector3(localScale.x, localScale.y, -localScale.z);
            Vector3 p2 = position + new Vector3(localScale.x, localScale.y, localScale.z);
            Vector3 p3 = position + new Vector3(-localScale.x, localScale.y, localScale.z);
            Vector3 p4 = position + new Vector3(-localScale.x, localScale.y, -localScale.z); 
            //Bottom Coners
            Vector3 p5 = position + new Vector3(localScale.x, -localScale.y, -localScale.z);
            Vector3 p6 = position + new Vector3(localScale.x, -localScale.y, localScale.z);
            Vector3 p7 = position + new Vector3(-localScale.x, -localScale.y, localScale.z);
            Vector3 p8 = position + new Vector3(-localScale.x, -localScale.y, -localScale.z); 
            //Top face
            _line.SetPosition(0,p1);
            _line.SetPosition(1,p2);
            _line.SetPosition(2,p3);
            _line.SetPosition(3,p4);
            _line.SetPosition(4,p1);
            //Bottom face
            _line.SetPosition(5,p5);
            _line.SetPosition(6,p6);
            _line.SetPosition(7,p7);
            _line.SetPosition(8,p8);
            _line.SetPosition(9,p5);
            //Side
            _line.SetPosition(10,p7);
            _line.SetPosition(11,p3);
            _line.SetPosition(12,p1);
            _line.SetPosition(13,p8);
            _line.SetPosition(14,p4);
            _line.SetPosition(15,p2);
            _line.SetPosition(16,p6);
            _line.SetPosition(17,p1);
            _line.SetPosition(18,p6);
            _line.SetPosition(19,p3);
            _line.SetPosition(20,p7);
            _line.SetPosition(21,p2);
            _line.SetPosition(22,p5);
            _line.SetPosition(23,p4);
            _line.SetPosition(24,p8);
            _line.SetPosition(25,p6);
        }
    }
}
