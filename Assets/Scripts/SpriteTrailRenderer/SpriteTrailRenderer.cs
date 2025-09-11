using System.Collections.Generic;
using UnityEngine;

namespace SpriteTrailRenderer
{
    public enum SpawnType
    {
        TIME,
        DISTANCE
    };

    public class SpriteTrailRenderer : MonoBehaviour
    {
        // time interval between spawns
        // alpha between spawn
        public Vector2 _startScale = Vector2.one;
        public Vector2 _endScale = Vector2.one;

        public float _trailLifetime = 1;
        public SpawnType _spawnType;

        public float _timeBetweenTrailSpawn = .5f;
        public float _distanceBetweenTrailSpawn;

        public bool _alphaUpdateOn;
        public float _maxAlpha;
        public float _minAlpha;

        public bool _useSolidColors;
        public bool _rainbowMode;
        public List<Color32> _userSelectedColorPalette;

        private SpriteRenderer _spriteRenderer;
        private float _lastSpawnTime;
        private Vector2 _lastSpawnLocation;
        private ObjectPool<SpriteTrailObject> _spriteObjects;

        private readonly static List<Color32> _rainbowColors = new List<Color32>
    {
        new Color32(0, 24, 255, 255), // blue
        new Color32(0, 157, 255, 255), // blue
        new Color32(0, 255, 246, 255), // indigo
        new Color32(0, 255, 166, 255), // indigo
        new Color32(0, 255, 0, 255), // green
        new Color32(72, 255, 0, 255), // green
        new Color32(125, 255, 0, 255), // green
        new Color32(255, 251, 0, 255), // yellow
        new Color32(255, 223, 0, 255), // yellow
        new Color32(255, 171, 0, 255), // orange
        new Color32(255, 120, 0, 255), // orange
        new Color32(255, 91, 0, 255), // orange
        new Color32(255, 0, 0, 255), // red
        new Color32(255, 0, 40, 255), // red
        new Color32(255, 0, 90, 255), // red
        new Color32(255, 0, 140, 255), // red
        new Color32(255, 0, 173, 255) // violet
    };


        // a fade alpha start value
        // a rainbow color setting
        // a pool private List<SpriteTrailOject> _trailSprites;


        private void Start()
        {
            //_trailSprites = new List<SpriteTrailObject>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _lastSpawnLocation = Vector2.zero;

            _spriteObjects = new ObjectPool<SpriteTrailObject>(SpawnTrailObject(), 3, 2);
        }


        private void Update()
        {
            if (_spawnType == SpawnType.TIME && Time.time - _lastSpawnTime >= _timeBetweenTrailSpawn)
            {
                SpriteTrailObject trailObject = _spriteObjects.GetObjectFromPool();
                SetValues(trailObject);
                trailObject.gameObject.SetActive(true);

                _lastSpawnTime = Time.time;
            }
            else if (_spawnType == SpawnType.DISTANCE && (_lastSpawnLocation - (Vector2)transform.position).magnitude > _distanceBetweenTrailSpawn)
            {
                SpriteTrailObject trailObject = _spriteObjects.GetObjectFromPool();
                SetValues(trailObject);
                trailObject.gameObject.SetActive(true);

                _lastSpawnLocation = transform.position;
            }
        }


        private GameObject SpawnTrailObject()
        {
            Vector3 newPosition = transform.position + Vector3.forward * .1f;
            GameObject newGameobject = new GameObject();

            newGameobject.AddComponent<SpriteRenderer>();
            newGameobject.AddComponent<SpriteTrailObject>();

            newGameobject.GetComponent<SpriteRenderer>().flipX = true;
            newGameobject.transform.position = newPosition;
            newGameobject.transform.eulerAngles = transform.eulerAngles;
            newGameobject.transform.localScale = transform.localScale;

            SpriteTrailObject trailObject = newGameobject.GetComponent<SpriteTrailObject>();
            trailObject.SetSpawnValues(_spriteRenderer, this, _rainbowMode ? _rainbowColors : _userSelectedColorPalette);
            newGameobject.name = "SpriteTrailObject";
            return newGameobject;
        }

        private void SetValues(SpriteTrailObject trailObject)
        {
            Vector3 newPosition = transform.position + Vector3.forward * .1f;

            trailObject.transform.position = newPosition;
            trailObject.transform.eulerAngles = transform.eulerAngles;
            trailObject.transform.localScale = transform.localScale;
            trailObject.SetSpawnValues(_spriteRenderer, this, _rainbowMode ? _rainbowColors : _userSelectedColorPalette);
        }
    }
}
