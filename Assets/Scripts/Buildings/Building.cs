using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Currency;

namespace Buildings
{
    public abstract class Building : ScriptableObject
    {
        [SerializeField]
        protected string id;
        [SerializeField]
        protected Vector2Int size;
        [SerializeField]
        protected List<CurrencyItem> price = new List<CurrencyItem>();

        public string Id
        {
            get { return id; }
        }

        public Vector2Int Size
        {
            get { return size; }
        }

        public List<CurrencyItem> Price
        {
            get { return price; }
        }

        public virtual void Build()
        {

        }

        public virtual void OnUpdate(int unixTime)
        {

        }

        public Building Clone()
        {
            return Instantiate(this);
        }
    }
}