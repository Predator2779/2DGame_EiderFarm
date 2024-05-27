using Unity.AI.Navigation;
using UnityEngine;
using EventHandler = General.EventHandler;

namespace Other
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class SurfaceBaker : MonoBehaviour
    {
        private NavMeshSurface _surface;

        private void Awake()
        {
            _surface = GetComponent<NavMeshSurface>();
            EventHandler.OnBakedNavigationSurface.AddListener(Bake);
        }

        private void Bake() => _surface.BuildNavMesh();
    }
}