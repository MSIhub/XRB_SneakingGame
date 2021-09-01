using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace StackACube
{
    public class CubeCornerDrawAndRayCast : MonoBehaviour
    {
        [SerializeField] private Transform _cube;

        [SerializeField] private LineRenderer _line;

        private List<Vector3> _cubeCorners = new List<Vector3>();
        public int noOfStars = 0;

        // Start is called before the first frame update
        void Start()
        {
            _line.positionCount = 26;
            ExtractCubeCorners();
            WireCube();
        }

        // Update is called once per frame
        private void Update()
        {
            noOfStars = StarRatingSystem();
        }

 

        private void ExtractCubeCorners()
        {
            Vector3 localScale = _cube.localScale * 0.5f;
            Vector3 centerPosition = _cube.position;
            //Top Corners
            _cubeCorners.Add(centerPosition + new Vector3(localScale.x, localScale.y, -localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(localScale.x, localScale.y, localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(-localScale.x, localScale.y, localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(-localScale.x, localScale.y, -localScale.z));
            //Bottom Corners
            _cubeCorners.Add(centerPosition + new Vector3(localScale.x, -localScale.y, -localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(localScale.x, -localScale.y, localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(-localScale.x, -localScale.y, localScale.z));
            _cubeCorners.Add(centerPosition + new Vector3(-localScale.x, -localScale.y, -localScale.z));
        }

        private void WireCube() //assuming rotation is zero
        {
            //Top face
            _line.SetPosition(0, _cubeCorners[0]);
            _line.SetPosition(1, _cubeCorners[1]);
            _line.SetPosition(2, _cubeCorners[2]);
            _line.SetPosition(3, _cubeCorners[3]);
            _line.SetPosition(4, _cubeCorners[0]);
            //Bottom face
            _line.SetPosition(5, _cubeCorners[4]);
            _line.SetPosition(6, _cubeCorners[5]);
            _line.SetPosition(7, _cubeCorners[6]);
            _line.SetPosition(8, _cubeCorners[7]);
            _line.SetPosition(9, _cubeCorners[4]);
            //Side
            _line.SetPosition(10, _cubeCorners[6]);
            _line.SetPosition(11, _cubeCorners[2]);
            _line.SetPosition(12, _cubeCorners[0]);
            _line.SetPosition(13, _cubeCorners[7]);
            _line.SetPosition(14, _cubeCorners[3]);
            _line.SetPosition(15, _cubeCorners[1]);
            _line.SetPosition(16, _cubeCorners[5]);
            _line.SetPosition(17, _cubeCorners[0]);
            _line.SetPosition(18, _cubeCorners[5]);
            _line.SetPosition(19, _cubeCorners[2]);
            _line.SetPosition(20, _cubeCorners[6]);
            _line.SetPosition(21, _cubeCorners[1]);
            _line.SetPosition(22, _cubeCorners[4]);
            _line.SetPosition(23, _cubeCorners[3]);
            _line.SetPosition(24, _cubeCorners[7]);
            _line.SetPosition(25, _cubeCorners[5]);
        }

        private bool[] CornerRayCast(out float[] distHit)
        {
            var centerPosition = _cube.position;
            var isTargetAreaFitted = new bool[_cubeCorners.Count];
            distHit = new float[_cubeCorners.Count];

            int i = 0;
            foreach (var cubeCorner in _cubeCorners)
            {
                if (Physics.Raycast(cubeCorner, (centerPosition - cubeCorner).normalized, out RaycastHit hit,
                    Vector3.Distance(cubeCorner, centerPosition) * 0.1f))
                {
                    isTargetAreaFitted[i] = true;
                    distHit[i] = hit.distance;
                }

                i++;
            }

            return isTargetAreaFitted;
        }
        
        private int StarRatingSystem()
        {
            var didHit = CornerRayCast(out var distanceOfHit);
            var noOfHit = didHit.Count(isHit => isHit);
            //var noOfStarsHit = Mathf.RoundToInt(noOfHit * 0.5f);
            var distPenalty = (from distHit in distanceOfHit
                where distHit > 0.0f
                where !(distHit <= 0.001f)
                select !(distHit > 0.001f) || !(distHit <= 0.002f)
                    ? distHit > 0.002f && distHit <= 0.003f ? 0.5f : 0.75f
                    : 0.25f).Sum();

            var noOfStars = Mathf.RoundToInt(noOfHit - distPenalty);
            // Ensuring the edge cases
            if (noOfStars > 4) noOfStars = 4;
            else if (noOfStars < 0) noOfStars = 0;
            return noOfStars;
        }
    }
}
//target area
/*private void OnDrawGizmos()
{
    Gizmos.color = Color.magenta;
    Gizmos.matrix = transform.localToWorldMatrix;
    Gizmos.DrawWireCube(_targetArea.position, _targetArea.localScale);
}*/
