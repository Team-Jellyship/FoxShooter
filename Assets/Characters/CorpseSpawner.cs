using UnityEngine;

namespace FoxShooter.Characters
{
    public class CorpseSpawner : MonoBehaviour
    {
        // Object to copy transforms from
        [SerializeField] private GameObject corpseSpriteObject;
        [SerializeField] private Sprite sprite;
        
        public void Spawn()
        {
            var corpse = Instantiate(corpseSpriteObject);
            corpse.transform.position = corpseSpriteObject.transform.position;
            corpse.name = $"{name}_Corpse";
            corpse.GetComponent<SpriteRenderer>().sprite = sprite;
            Destroy(gameObject);
        }
    }
}