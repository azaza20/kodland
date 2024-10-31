using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathCreation
{
    public class AI : MonoBehaviour
    {
        public PathCreator pathCreator;
        public Car car { get; private set; }

        public float distanseThreshold;
        public int nextPoint;
        public int S_nextPoint;

        public float sensorFrontOffset;
        public float sensorSideOffset;
        public float sensorEndShift;
        public float sensorLength;
        public float smallsensorendshift;
        public float smallsensorlength;

        private Rigidbody rb;

        public Transform sensorStart;

        public Transform S_transform;

        public bool stop;

        [Range(2, 4)]
        public float stiffness = 3f;
        [Range(160, 230)]
        public int CarPower = 180;
        [Range(20, 50)]
        public int MaxSteerAngle = 30;
        [Range(180, 220)]
        public int MaxSpeed = 200;
        [Range(125, 50)]
        public int BrakingDistance = 100;
        [Range(0.15f, .35f)]
        public float BrakingMp = 0.25f;
        [Range(1.5f, .5f)]
        public float BreakMp = 1f;
        [SerializeField] float motorSpeed;
        [SerializeField] float MaxWheel;
        private void Awake()
        {
            car = GetComponent<Car>();
            FindClosestWaypoint();
            rb = GetComponent<Rigidbody>();
        }
        public void FindClosestWaypoint()
        {
            float closestDistanse = float.MaxValue;
            int closestWaypoint = -1;

            Vector3 myPos = transform.position;
            for (int i = 0; i < pathCreator.path.NumPoints; i++)
            {
                float distanse = Vector3.Distance(pathCreator.path.GetPoint(i), myPos);
                if (distanse < closestDistanse)
                {
                    closestDistanse = distanse;
                    closestWaypoint = i;
                }
            }

            nextPoint = closestWaypoint;
        }

        Vector3 GetSensorStart(float dir)
        {
            return sensorStart.transform.position + sensorStart.transform.forward * sensorFrontOffset + sensorStart.transform.right * sensorSideOffset * dir;
        }
        Vector3 GetSensorDir(float dir, float shift)
        {
            return sensorStart.transform.forward * sensorLength + sensorStart.transform.right * shift * sensorEndShift;
        }
        Vector3 getsmallsensordir(float dir, float shift)
        {
            return sensorStart.transform.forward * smallsensorlength + sensorStart.transform.right * shift * smallsensorendshift;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(pathCreator.path.GetPoint(nextPoint), Vector3.one);
            Gizmos.DrawCube(pathCreator.path.GetPoint(S_nextPoint), Vector3.one);

            Gizmos.color = Color.green;

            Gizmos.DrawRay(GetSensorStart(0.0f), GetSensorDir(0.0f, 0.0f));
            Gizmos.DrawRay(GetSensorStart(-1.0f), GetSensorDir(-1.0f, -1.0f));
            Gizmos.DrawRay(GetSensorStart(-1.0f), GetSensorDir(-1.0f, 0.0f));
            Gizmos.DrawRay(GetSensorStart(1.0f), GetSensorDir(1.0f, 1.0f));
            Gizmos.DrawRay(GetSensorStart(1.0f), GetSensorDir(1.0f, 0.0f));

            Gizmos.DrawRay(GetSensorStart(0.0f), getsmallsensordir(0.0f, 0.0f));
            Gizmos.DrawRay(GetSensorStart(-1.0f), getsmallsensordir(-1.0f, -1.0f));
            Gizmos.DrawRay(GetSensorStart(-1.0f), getsmallsensordir(-1.0f, 0.0f));
            Gizmos.DrawRay(GetSensorStart(1.0f), getsmallsensordir(1.0f, 1.0f));
            Gizmos.DrawRay(GetSensorStart(1.0f), getsmallsensordir(1.0f, 0.0f));
        }
        private float T;
        void Update()
        {
            T += Time.deltaTime;
            if (nextPoint == 1 && T > 1)
            {
                Debug.Log(T);
                T = 0;
            }
            Vector3 targetWaypount = pathCreator.path.GetPoint(nextPoint);
            Vector3 target = new Vector3(targetWaypount.x, transform.position.y, targetWaypount.z);
            Vector3 vectorToTarget = transform.InverseTransformPoint(target);
            float distanseToTarger = vectorToTarget.magnitude;

            if (distanseToTarger <= distanseThreshold)
            {
                nextPoint = (nextPoint + 1) % pathCreator.path.NumPoints;
            }
            S_transform.position = pathCreator.path.GetPoint(nextPoint);
            S_transform.rotation = pathCreator.path.GetRotation(nextPoint);
            Vector3 S_targetWaypount = pathCreator.path.GetPoint(S_nextPoint);
            Vector3 S_target = new Vector3(S_targetWaypount.x, S_transform.position.y, S_targetWaypount.z);
            Vector3 S_vectorToTarget = S_transform.InverseTransformPoint(S_target);
            float S_distanseToTarger = S_vectorToTarget.magnitude;

            float speed = MaxSpeed / 2.7f;
            speed = 1 / speed * rb.velocity.magnitude;

            if (S_distanseToTarger <= BrakingDistance)
            {
                S_nextPoint = (S_nextPoint + 1) % pathCreator.path.NumPoints;
            }
            Vector3 SS_vectorToTarget = transform.InverseTransformPoint(S_target);
            float SS_distanseToTarger = SS_vectorToTarget.magnitude;
            float forward = 0;
            float tg = SS_vectorToTarget.x / SS_distanseToTarger;
            if (tg < 0)
            {
                tg *= -1;
            }
            if (tg > BrakingMp && speed > BrakingMp)
            {
                forward = (1.75f - tg - speed - speed) * BreakMp;
            }
            else
            {
                forward = 1;
            }
            float steer = vectorToTarget.x / distanseToTarger;
            RaycastHit hit;
            if (Physics.Raycast(GetSensorStart(0.0f), GetSensorDir(0.0f, 0.0f), out hit, sensorLength))
            {
                if (hit.rigidbody != null)
                {
                    if (rb.velocity.magnitude >= hit.rigidbody.velocity.magnitude + 3)
                    {
                        forward = -1;
                    }
                }
                else
                {
                    forward = -1;
                }
            }
            //if (Physics.Raycast(GetSensorStart(-1.0f), GetSensorDir(-1.0f, -1.0f), out hit, sensorLength))
            //{
            //    if (hit.rigidbody != null)
            //    {
            //        if (rb.velocity.magnitude >= hit.rigidbody.velocity.magnitude + 3)
            //        {
            //            steer = 1;
            //        }
            //    }
            //    else
            //    {
            //        steer = 1;
            //    }
            //}
            //if (Physics.Raycast(GetSensorStart(-1.0f), GetSensorDir(-1.0f, 0.0f), out hit, sensorLength))
            //{
            //    if (hit.rigidbody != null)
            //    {
            //        if (rb.velocity.magnitude >= hit.rigidbody.velocity.magnitude + 3)
            //        {
            //            steer = 1;
            //        }
            //    }
            //    else
            //    {
            //        steer = 1;
            //    }

            //}
            //if (Physics.Raycast(GetSensorStart(1.0f), GetSensorDir(1.0f, 1.0f), out hit, sensorLength))
            //{
            //    if (hit.rigidbody != null)
            //    {
            //        if (rb.velocity.magnitude >= hit.rigidbody.velocity.magnitude + 3)
            //        {
            //            steer = -1;
            //        }
            //    }
            //    else
            //    {
            //        steer = -1;
            //    }

            //}
            //if (Physics.Raycast(GetSensorStart(1.0f), GetSensorDir(1.0f, 0.0f), out hit, sensorLength))
            //{
            //    if (hit.rigidbody != null)
            //    {
            //        if (rb.velocity.magnitude >= hit.rigidbody.velocity.magnitude + 3)
            //        {
            //            steer = -1;
            //        }
            //    }
            //    else
            //    {
            //        steer = -1;
            //    }

            //}
            //if (Physics.Raycast(GetSensorStart(0.0f), getsmallsensordir(0.0f, 0.0f), smallsensorlength))
            //{
            //    forward = -1;
            //}
            //if (Physics.Raycast(GetSensorStart(-1.0f), getsmallsensordir(-1.0f, -1.0f), smallsensorlength + 1))
            //{
            //    steer = 1;
            //}
            //if (Physics.Raycast(GetSensorStart(-1.0f), getsmallsensordir(-1.0f, 0.0f), smallsensorlength + 1))
            //{
            //    steer = 1;
            //}
            //if (Physics.Raycast(GetSensorStart(1.0f), getsmallsensordir(1.0f, 1.0f), smallsensorlength + 1))
            //{
            //    steer = -1;
            //}
            //if (Physics.Raycast(GetSensorStart(1.0f), getsmallsensordir(1.0f, 0.0f), smallsensorlength + 1))
            //{
            //    steer = -1;
            //}
            car.motor = motorSpeed * forward;
            car.Angle = steer * MaxWheel;

        }
    }
}