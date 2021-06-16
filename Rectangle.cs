using System;
using System.Numerics;

namespace YARL
{
   public struct Rectangle : IEquatable<Rectangle>
   {
      public int Height { get; set; }

      public int Width { get; set; }

      public int X { get; set; }

      public int Y { get; set; }


      #region Constructors

      public Rectangle( int x, int y, int width, int height )
         : this()
      {
         X = x;
         Y = y;
         Width = width;
         Height = height;
      }

      public Rectangle( Vector2 location, Vector2 size )
         : this()
      {
         X = (int) location.X;
         Y = (int) location.Y;
         Width = (int) size.X;
         Height = (int) size.Y;
      }

      #endregion

      public static Rectangle Empty { get; } = new Rectangle();

      public int Left => X;

      public int Right => X + Width;

      public int Top => Y;

      public int Bottom => Y + Height;

      public Vector2 Location
      {
         get => new Vector2( X, Y );
         set
         {
            X = (int) value.X;
            Y = (int) value.Y;
         }
      }

      public Vector2 Center => new Vector2( X + ( Width / 2 ), Y + ( Height / 2 ) );

      public bool IsEmpty => Width == 0 && Height == 0 && X == 0 && Y == 0;

      public bool Equals( Rectangle other )
      {
         return this == other;
      }

      public static bool operator ==( Rectangle a, Rectangle b )
      {
         return ( a.X == b.X ) && ( a.Y == b.Y ) && ( a.Width == b.Width ) && ( a.Height == b.Height );
      }

      public bool Contains( int x, int y )
      {
         return ( X <= x ) && ( x < ( X + Width ) ) && ( Y <= y ) && ( y < ( Y + Height ) );
      }

      public bool Contains( Vector2 value )
      {
         return ( X <= value.X ) && ( value.X < ( X + Width ) ) && ( Y <= value.Y ) && ( value.Y < ( Y + Height ) );
      }

      public bool Contains( Rectangle value )
      {
         return ( X <= value.X ) && ( ( value.X + value.Width ) <= ( X + Width ) ) && ( Y <= value.Y )
                  && ( ( value.Y + value.Height ) <= ( Y + Height ) );
      }

      public static bool operator !=( Rectangle a, Rectangle b )
      {
         return !( a == b );
      }

      public void Offset( Vector2 offsetPoint )
      {
         X += (int) offsetPoint.X;
         Y += (int) offsetPoint.Y;
      }

      public void Offset( int offsetX, int offsetY )
      {
         X += offsetX;
         Y += offsetY;
      }

      public void Inflate( int horizontalValue, int verticalValue )
      {
         X -= horizontalValue;
         Y -= verticalValue;
         Width += horizontalValue * 2;
         Height += verticalValue * 2;
      }

      public override bool Equals( object obj )
      {
         return ( obj is Rectangle ) && this == ( (Rectangle) obj );
      }

      public override string ToString()
      {
         return $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
      }

      public override int GetHashCode()
      {
         return X ^ Y ^ Width ^ Height;
      }

      public bool Intersects( Rectangle value )
      {
         return value.Left < Right &&
                Left < value.Right &&
                value.Top < Bottom &&
                Top < value.Bottom;
      }

      public void Intersects( ref Rectangle value, out bool result )
      {
         result = value.Left < Right &&
                  Left < value.Right &&
                  value.Top < Bottom &&
                  Top < value.Bottom;
      }

      public static Rectangle Intersect( Rectangle value1, Rectangle value2 )
      {
         Intersect( ref value1, ref value2, out Rectangle rectangle );
         return rectangle;
      }

      public static void Intersect( ref Rectangle value1, ref Rectangle value2, out Rectangle result )
      {
         if ( value1.Intersects( value2 ) )
         {
            int rightSide = Math.Min( value1.X + value1.Width, value2.X + value2.Width );
            int leftSide = Math.Max( value1.X, value2.X );
            int topSide = Math.Max( value1.Y, value2.Y );
            int bottomSide = Math.Min( value1.Y + value1.Height, value2.Y + value2.Height );
            result = new Rectangle( leftSide, topSide, rightSide - leftSide, bottomSide - topSide );
         }
         else
         {
            result = new Rectangle( 0, 0, 0, 0 );
         }
      }

      public static Rectangle Union( Rectangle value1, Rectangle value2 )
      {
         int x = Math.Min( value1.X, value2.X );
         int y = Math.Min( value1.Y, value2.Y );
         return new Rectangle( x, y, Math.Max( value1.Right, value2.Right ) - x, Math.Max( value1.Bottom, value2.Bottom ) - y );
      }

      public static void Union( ref Rectangle value1, ref Rectangle value2, out Rectangle result )
      {
         result = Union( value1, value2 );
      }
   }
}
