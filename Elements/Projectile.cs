using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideShooting.Elements
{
    /// <summary>
    /// Contiene los valores y métodos de los proyectiles que se dispara en el juego
    /// </summary>
    public class Projectile
    {
        /// <summary>
        /// Contador para generar una <see cref="Id"/> para la clase
        /// </summary>
        private static int idCounter = 0;

        /// <summary>
        /// ID que tiene un proyectil, para poder distinguir unos de otros al enviarlos al servidor
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Posición actual de un proyectil
        /// </summary>
        public Vector2 Location { get; set; }
        /// <summary>
        /// Valores que se deben sumar a <see cref="Location"/> en cada tick del juego
        /// </summary>
        public Vector2 Acceleration { get; set; }
        /// <summary>
        /// Velocidad en píxeles a la que va el proyectil
        /// </summary>
        /// <seealso cref="Acceleration"/>
        public int Speed { get; set; } = 4;
        /// <summary>
        /// <see cref="Texture2D"/> del proyectil y sus cuadros
        /// </summary>
        public Texture2D Sprite { get; set; }
        /// <summary>
        /// Frame que se debe dibujar de <see cref="Sprite"/>
        /// </summary>
        public int CurrentFrame { get; set; } = 0;

        /// <summary>
        /// Lado que tiene el proyectil
        /// </summary>
        public int side = 20;//lo meto a piñon

        /// <summary>
        /// Valor que indica el tiempo desde el último cambio de <see cref="CurrentFrame"/>
        /// </summary>
        private float timeSinceLastFrameStep = 0;
        /// <summary>
        /// Valor máximo de frames/cuadros
        /// </summary>
        /// <seealso cref="Sprite"/>
        private int maxFrames = 4;

        /// <summary>
        /// Inicializa un nuevo proyectil
        /// </summary>
        /// <remarks>Este constructor está pensado para usarse en la creación de proyectiles
        /// generados por pulsaciones del jugador local.</remarks>
        /// <param name="sprite">Textura que tiene el proyectil</param>
        /// <param name="playerLocation">Posición inicial desde la que parte el proyectil</param>
        /// <param name="mouseLocation">Trayectoria que debe seguir el proyectil</param>
        public Projectile(Texture2D sprite, Vector2 playerLocation, Point mouseLocation)
        {
            this.Sprite = sprite;

            double atan2 = Math.Atan2(playerLocation.Y - mouseLocation.Y, playerLocation.X - mouseLocation.X);
            //double angle = (atan > 0 ? atan : (2 * Math.PI + atan)) * 360 / (2 * Math.PI);
            double radians = atan2 > 0 ? atan2 : 2 * Math.PI + atan2;

            this.Acceleration = new Vector2((float) -Math.Cos(radians) * Speed, (float) -Math.Sin(radians) * Speed);
            this.Location = playerLocation + this.Acceleration * 5;

            this.Id = idCounter;
            idCounter++;

            if (idCounter >= 10000)
            {
                idCounter = 0;
            }
        }

        /// <summary>
        /// Inicializa un nuevo proyectil
        /// </summary>
        /// <remarks>Este constructor está pensado para usarse en la creación de proyectiles
        /// generados por otros jugadores con información recibida vía servidor.</remarks>
        /// <param name="sprite">Texturas del proyectil</param>
        /// <param name="location">Posición inicial desde la que parte el proyectil</param>
        /// <param name="acceleration">Valores de la trayectoria a seguir por el proyectil</param>
        /// <param name="id">Id que debe tener el proyectil para identificarlo en cada cliente</param>
        public Projectile(Texture2D sprite, Vector2 location, Vector2 acceleration, int id)
        {
            this.Sprite = sprite;
            this.Location = location;
            this.Acceleration = acceleration;
            this.Id = id;
        }

        /// <summary>
        /// Actualiza la posición del proyectil y cambia su <see cref="CurrentFrame"/> en caso de ser necesario
        /// </summary>
        /// <param name="gameTime">Provee de los valores actuales de tiempo</param>
        public void Update(GameTime gameTime)
        {
            this.Location += this.Acceleration;
            //System.Diagnostics.Debug.WriteLine("Location " + this.Location);

            timeSinceLastFrameStep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrameStep > 0.15)
            {
                CurrentFrame++;
                timeSinceLastFrameStep = 0;

                if (CurrentFrame >= maxFrames)
                {
                    CurrentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Dibuja el proyectil.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> que se usa en la clase principal del juego</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(1, (1 * (CurrentFrame + 1) + side * CurrentFrame) + 1, side, side);

            spriteBatch.Draw(Sprite, Location, sourceRectangle, Color.White);
        }
    }
}
