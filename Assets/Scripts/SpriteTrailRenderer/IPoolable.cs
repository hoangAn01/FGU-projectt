namespace SpriteTrailRenderer
{
    public interface IPoolable
    {
        void SetReturnToPool(ReturnObjectToPool returnDelegate);
    }
}