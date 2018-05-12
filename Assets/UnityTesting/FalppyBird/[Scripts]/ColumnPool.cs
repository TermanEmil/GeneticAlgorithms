using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GA_Tests.FlappyBird
{
    public class ColumnPool : MonoBehaviour
    {
        static private ColumnPool instance;
        static public ColumnPool Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<ColumnPool>();
                return instance;
            }
        }

        public float scrollSpeed = 1;
        public float distBetweenColumns = 5;
        public Transform visibilityPoint = null;
        [SerializeField] private Transform nonVisibilityPoint = null;

        private List<Column> columnPool = new List<Column>();
        private Transform lastColumn = null;

        void Start()
        {
            foreach (Transform child in transform.Find("Columns"))
            {
                var column = child.GetComponent<Column>();
                if (column != null)
                {
                    columnPool.Add(column);
                    column.gameObject.SetActive(false);
                }
            }
        }

        public Transform ClossestColumn(Vector3 pos)
        {
            float min = Mathf.Infinity;
            Transform result = null;

            foreach (var column in columnPool)
            {
                if ((column.gameObject.activeSelf) &&
                    (column.transform.position.x > pos.x))
                {
                    var delta = column.transform.position.x - pos.x;
                    if (delta < min)
                    {
                        min = delta;
                        result = column.transform;
                    }
                }
            }
            return result;
        }

        public void ResetColumns()
        {
            columnPool.ForEach(x => x.gameObject.SetActive(false));
            UpdateColumns();
        }

        private void FixedUpdate()
        {
            UpdateColumns();
        }

        public void UpdateColumns()
        {
            if (lastColumn == null ||
                (lastColumn.position.x + distBetweenColumns <
                 visibilityPoint.position.x))
            {
                var newColumn = GetFreeColumn();
                var pos = transform.position;

                if (lastColumn == null)
                    pos.x = visibilityPoint.position.x;
                else
                    pos.x = lastColumn.position.x + distBetweenColumns;
                newColumn.position = pos;
                newColumn.gameObject.SetActive(true);
                lastColumn = newColumn;
            }

            foreach (var column in columnPool)
            {
                column.transform.Translate(Vector3.left *
                                           scrollSpeed *
                                           Time.fixedDeltaTime);
                if (column.transform.position.x < nonVisibilityPoint.position.x)
                    column.gameObject.SetActive(false);
            }
        }

        private Transform GetFreeColumn()
        {
            return columnPool.First(x => !x.gameObject.activeSelf).transform;
        }
    }
}