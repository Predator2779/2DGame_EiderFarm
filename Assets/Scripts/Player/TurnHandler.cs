using UnityEngine;

namespace Player
{
    public class TurnHandler
    {
        private SpriteRenderer _renderer;
        public TurnHandler(SpriteRenderer renderer) { _renderer = renderer; }

        public void ChangeSide(PlayerSides side)
        {
            switch (side)
            {
                case PlayerSides.Left:
                    RotateObj(_renderer.transform, 180);
                    break;
                default:
                    RotateObj(_renderer.transform, 0);
                    break;
            }
        }

        private void RotateObj(Transform obj, float angle) => obj.localRotation = Quaternion.Euler(0f, angle, 0f);

        public enum PlayerSides
        {
            Left,
            Right
        }
    }
}