using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SideShooting.Elements
{
    /// <summary>
    /// Tipo que indica los parámetros y métodos de los personajes en el juego
    /// </summary>
    public class Character
    {
        //Constantes
        /// <summary>
        /// Indica el máximo de salud posible para el personaje
        /// </summary>
        public const int MaxHealth = 6;
        /// <summary>
        /// Indica el máximo de la barra verde/acelerón del personaje
        /// </summary>
        public const int MaxGreen = 50;
        /// <summary>
        /// Indica el máximo de la barra azul/fogueo del personaje
        /// </summary>
        public const int MaxBlue = 100;
        /// <summary>
        /// Indica el número máximo de proyectiles que un personaje puede disparar a la vez
        /// </summary>
        public const int MaxProjectiles = 9;

        //Propiedades
        /// <summary>
        /// Textura con los sprites del personaje
        /// </summary>
        public Texture2D Sprite { get; set; }
        /// <summary>
        /// Indica el valor de que tipo de animación/movimiento se ha de coger de <see cref="Sprite"/>
        /// </summary>
        /// <remarks>0: abajo. 1: derecha. 2: arriba. 3: izquierda</remarks>
        public int CurrentAnimation { get; set; }
        /// <summary>
        /// Indica el valor actual del frame que se está dibujando de una animación
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// Delimita los bordes de cada una de las animaciones
        /// </summary>
        /// <remarks>0: abajo. 1: derecha. 2: arriba. 3: izquierda</remarks>
        public Rectangle[] SourceAnimations { get; set; }
        /// <summary>
        /// Indica la posición en el mapa del personaje
        /// </summary>
        public Vector2 Location { get; set; }
        /// <summary>
        /// Indica cuantos frames de animación de daño (pantallazo rojo) han de dibujarse (la mitad de este valor)
        /// </summary>
        public int DamageEffect { get; set; } = 0;
        /// <summary>
        /// Indica la salud actual que tiene el personaje
        /// </summary>
        /// <seealso cref="MaxHealth"/>
        public int Health { get; set; }
        /// <summary>
        /// Indica el valor actual de la barra verde/acelerón del personaje
        /// </summary>
        /// <seealso cref="MaxGreen"/>
        public int GreenBar { get; set; }
        /// <summary>
        /// Indica el valor actual de la barra azul del personaje
        /// </summary>
        /// <seealso cref="MaxBlue"/>
        public int BlueBar { get; set; }
        /// <summary>
        /// Indica cuantos proyectiles puede disparar el personaje
        /// </summary>
        /// <seealso cref="MaxProjectiles"/>
        public int ProjectilesAvailable { get; set; }

        //Variables públicas
        /// <summary>
        /// Indica el alto del <see cref="Sprite"/>
        /// </summary>
        public int spriteHeight = (int) (22 * 1.3);
        /// <summary>
        /// Indica el ancho del <see cref="Sprite"/>
        /// </summary>
        public int spriteWidth = (int) (15 * 1.3);
        /// <summary>
        /// Velocidad en píxeles a la que se mueve el personaje
        /// </summary>
        public int speed = 2;

        //Variables privadas
        /// <summary>
        /// Indica el máximo de frames de sus animaciones
        /// </summary>
        /// <seealso cref="CurrentFrame"/>
        private int maxFrames;
        /// <summary>
        /// El tiempo que ha transcurrido desde el cambio del último frame a dibujar
        /// </summary>
        private float timeSinceLastFrameStep = 0;

        /// <summary>
        /// Crea una nueva instancia de un personaje
        /// </summary>
        /// <param name="sprite"></param>
        public Character(Texture2D sprite)
        {
            this.Sprite = sprite;
            CurrentFrame = 1;
            maxFrames = 3;
            Health = MaxHealth;
            GreenBar = MaxGreen;
            BlueBar = MaxBlue;
            ProjectilesAvailable = MaxProjectiles;

            SourceAnimations = new Rectangle[4];
            for (int i = 0, left = 1, top = 6, width = 64, height = 22; i < SourceAnimations.Length; i++, top += 32)
            {
                SourceAnimations[i] = new Rectangle(left, top, width, height);
            }

            SourceAnimations[0] = new Rectangle(1, 6, 64, 22);//separacion de 1px entre imagenes, 15*4 + 4 de ese px
            CurrentAnimation = 0;
            Location = new Vector2(20);
        }

        /// <summary>
        /// Aquí se dibujará al personaje según su <see cref="CurrentAnimation"/> y <see cref="CurrentFrame"/>
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> que se usa en la clase principal del juego</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            int width = SourceAnimations[CurrentAnimation].Width / (maxFrames + 1) - 1;

            Rectangle sourceRectangle = new Rectangle(SourceAnimations[CurrentAnimation].Left + width * (CurrentFrame + 1) + 1 * (CurrentFrame + 1),
                SourceAnimations[CurrentAnimation].Top, width, SourceAnimations[CurrentAnimation].Height);
            Rectangle destinationRectangle = new Rectangle((int)Location.X, (int)Location.Y, width, SourceAnimations[CurrentAnimation].Height);

            //spriteBatch.Draw(Sprite, Location, sourceRectangle, Color.White);
            spriteBatch.Draw(Sprite, new Rectangle((int)Location.X, (int)Location.Y, spriteWidth, spriteHeight), sourceRectangle, Color.White);
            //spriteBatch.Draw(Sprite, destinationRectangle, sourceRectangle, Color.White);
        }

        /// <summary>
        /// Aquí se ajustará el cambio de frame si fuese necesario, y los valores de sus barras roja/verde/azul
        /// </summary>
        /// <param name="gameTime">Provee de los valores actuales de tiempo</param>
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrameStep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastFrameStep > 0.15)
            {
                if (speed > 2 && GreenBar > 0)
                {
                    GreenBar -= 5;

                    if (GreenBar <= 0)
                    {
                        speed = 2;
                    }
                }
                else if (GreenBar < MaxGreen)
                {
                    GreenBar++;
                }

                if (BlueBar < MaxBlue)
                {
                    BlueBar++;
                }

                CurrentFrame++;
                timeSinceLastFrameStep = 0;

                if (CurrentFrame == maxFrames)
                {
                    CurrentFrame = 0;
                }
            }
        }
    }
}
