using UnityEngine;

public class EnemyStatus : MonoBehaviour, IShootable
{
    [SerializeField] float _lifeMax = 2;
    private float _currentLife;
    [SerializeField] private GameObject _bloodEffect;
    public void Hitted(float damage, Vector3 shootPoint)
    {
        _currentLife -= damage;
      
        GameObject blood = Instantiate(_bloodEffect, shootPoint, Quaternion.LookRotation(shootPoint - transform.position));
        blood.transform.SetParent(transform);
        
        if (_currentLife > 0)
            return;
        Destroy(gameObject); 
    }

    
    void Start()
    {
        _currentLife = _lifeMax;
    }
}
