using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class FPSCounter : MonoBehaviour
    {
        public  float updateInterval = 0.5F;

        private float accum   = 0; // FPS accumulated over the interval
        private int   frames  = 0; // Frames drawn over the interval
        private float timeleft; // Left time for current interval
        Text guiText_custom;

        void Start()
        {
            guiText_custom = GetComponent<Text>();
            if( !guiText_custom )
            {
                Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
                enabled = false;
                return;
            }
            timeleft = updateInterval;  
        }

        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale/Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if( timeleft <= 0.0 )
            {
                // display two fractional digits (f2 format)
                float fps = accum/frames;
                string format = System.String.Format("{0:F2} FPS",fps);
                guiText_custom.text = format;

                if(fps < 30)
                    guiText_custom.color = Color.yellow;
                else 
                    if(fps < 10)
                        guiText_custom.color = Color.red;
                    else
                        guiText_custom.color = Color.green;
                //  DebugConsole.Log(format,level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }

            if (Input.GetMouseButtonDown(0))
            {
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //hit.collider.renderer.material.color = Color.red;
                    Debug.Log(hit.collider.gameObject.name);
                }

            }
        }
    }
}
