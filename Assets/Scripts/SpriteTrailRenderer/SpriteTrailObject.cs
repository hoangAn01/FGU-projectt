using System.Collections.Generic;
using UnityEngine;

namespace SpriteTrailRenderer
{
    public class SpriteTrailObject : MonoBehaviour, IPoolable
    {
        public ReturnObjectToPool _returnToPool;

        public SpriteTrailRenderer _spriteTrailValues;
        private SpriteRenderer _spriteRenderer;
        private float _timeAlive;
        private bool _active;


        // have a color palette that is passed into the sprite trail object
        // allow the user to input their own color variations
        public List<Color32> _colorsToUse;

        private void Awake()
        {
            _returnToPool = (x) => x.SetActive(false);
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _timeAlive = 0f;
            _active = false;
        }


        private void Update()
        {
            if (_active)
            {
                _timeAlive += Time.deltaTime;
                float percentDone = Mathf.Min(_timeAlive / _spriteTrailValues._trailLifetime, .999f);
                Vector3 newZPosition = transform.position;
                newZPosition.z = (float)(_spriteTrailValues.transform.position.z + (.01 * percentDone));
                transform.position = newZPosition;

                Color newColor = _spriteRenderer.color;

                // update scales
                float newXScale = Mathf.Lerp(_spriteTrailValues._startScale.x, _spriteTrailValues._endScale.x, percentDone);
                newXScale = transform.localScale.x < 0 ? newXScale * -1 : newXScale;

                float newYScale = Mathf.Lerp(_spriteTrailValues._startScale.y, _spriteTrailValues._endScale.y, percentDone);
                newYScale = transform.localScale.y < 0 ? newYScale * -1 : newYScale;

                transform.localScale = new Vector2(newXScale, newYScale);

                // just do color mode
                if (_spriteTrailValues._useSolidColors)
                {
                    newColor = _colorsToUse[Mathf.FloorToInt(_colorsToUse.Count * percentDone)];
                }

                if (_spriteTrailValues._alphaUpdateOn)
                {
                    // max - min
                    newColor.a = ((1f - percentDone) * (_spriteTrailValues._maxAlpha - _spriteTrailValues._minAlpha)) + _spriteTrailValues._minAlpha;
                }

                _spriteRenderer.color = newColor;

                if (_timeAlive >= _spriteTrailValues._trailLifetime)
                {
                    _active = false;
                    _returnToPool(gameObject);
                }
            }
        }

        public void SetSpawnValues(SpriteRenderer spriteRenderer, SpriteTrailRenderer spriteTrailRenderer, List<Color32> colors)
        {
            _spriteTrailValues = spriteTrailRenderer;

            _spriteRenderer.flipX = spriteRenderer.flipX;
            _spriteRenderer.flipY = spriteRenderer.flipY;
            _spriteRenderer.sprite = spriteRenderer.sprite;

            _colorsToUse = colors;

            if (_spriteTrailValues._useSolidColors)
            {
                Shader shaderGUItext = Shader.Find("GUI/Text Shader");
                _spriteRenderer.material.shader = shaderGUItext;
                _spriteRenderer.color = Color.clear;
            }
            else
            {
                // use default shader
                Shader spriteShader = Shader.Find("Sprites/Default");
                _spriteRenderer.material.shader = spriteShader;
                _spriteRenderer.color = spriteRenderer.color;
            }

            _timeAlive = 0f;
            _active = true;
        }

        public void SetReturnToPool(ReturnObjectToPool returnDelegate)
        {
            _returnToPool = returnDelegate;
        }
    }
}