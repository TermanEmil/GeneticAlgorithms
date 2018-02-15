using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA_Tests.FlappyBird
{
    public class Column : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D topCollider = null;
        [SerializeField] private BoxCollider2D botCollider = null;
        [SerializeField] private BoxCollider2D centerTrigger = null;
        [HideInInspector] public float size = 1;

        public void SetColumnSize(float newSize)
        {
            var yPos = transform.position.y + newSize;
            SetBlockPos(topCollider, 1, yPos);
            SetBlockPos(botCollider, -1, yPos);

            var triggerPos = centerTrigger.transform.localPosition;
            triggerPos.y = 0;
            centerTrigger.transform.localPosition = triggerPos;

            var topPoint =
                topCollider.transform.position.y - topCollider.bounds.extents.y;
            var botPoint =
                botCollider.transform.position.y + botCollider.bounds.extents.y;
            var triggerSize = centerTrigger.size;
            triggerSize.y = Mathf.Abs(topPoint - botPoint);
            centerTrigger.size = triggerSize;
        }

        private void SetBlockPos(BoxCollider2D box, int direction, float yPos)
        {
            var prevPos = box.transform.position;
            prevPos.y = direction * (yPos + box.bounds.extents.y);
            box.transform.position = prevPos;
        }
    }
}
