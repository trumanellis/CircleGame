using UnityEngine;
using System;
using System.Collections;

namespace Ignis.Utils.Math {
    public class Rounding {
        public static float RoundToPlaces(float value, int places) {
            float mult = Mathf.Pow(10, places);
            return Mathf.Round(value * mult) / mult;
        }

        public static float RoundToNearest(float value, float nearest) {
            return Mathf.Round(value / nearest) * nearest;
        }
    }

    public class Random {
        private static System.Random random;
        public static int Seed { set { random = new System.Random(value); } }

        public static bool RandomBool() {
            return random == null ? (UnityEngine.Random.Range(0, 100) % 2 == 0) : (random.Next(0, 100) % 2 == 0);
        }
    }
}

namespace Ignis.Util.Colors {
    /// <summary>
    /// Structure to define HSBColor.
    /// </summary>
    public struct HSB {
        /// <summary>
        /// Gets an empty HSBColor structure;
        /// </summary>
        public static readonly HSB Empty = new HSB();

        private float hue;
        private float saturation;
        private float brightness;

        public static bool operator ==(HSB item1, HSB item2) {
            return (
                item1.Hue == item2.Hue
                && item1.Saturation == item2.Saturation
                && item1.Brightness == item2.Brightness
                );
        }

        public static bool operator !=(HSB item1, HSB item2) {
            return (
                item1.Hue != item2.Hue
                || item1.Saturation != item2.Saturation
                || item1.Brightness != item2.Brightness
                );
        }

        /// <summary>
        /// Gets or sets the hue component.
        /// </summary>
        public float Hue {
            get {
                return hue;
            }
            set {
                hue = (value > 360) ? 360 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets saturation component.
        /// </summary>
        public float Saturation {
            get {
                return saturation;
            }
            set {
                saturation = (value > 1) ? 1 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets the brightness component.
        /// </summary>
        public float Brightness {
            get {
                return brightness;
            }
            set {
                brightness = (value > 1) ? 1 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Creates an instance of a HSBColor structure.
        /// </summary>
        /// <param name="h">Hue value.</param>
        /// <param name="s">Saturation value.</param>
        /// <param name="b">Brightness value.</param>
        public HSB(float h, float s, float b) {
            hue = (h > 360) ? 360 : ((h < 0) ? 0 : h);
            saturation = (s > 1) ? 1 : ((s < 0) ? 0 : s);
            brightness = (b > 1) ? 1 : ((b < 0) ? 0 : b);
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (HSB)obj);
        }

        public override int GetHashCode() {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
                Brightness.GetHashCode();
        }

        public static HSB ToHSBColor(Color32 color) {
            return ToHSBColor(color.r, color.g, color.b);
        }

        /// <summary>
        /// Converts RGB to HSB.
        /// </summary>
        public static HSB ToHSBColor(int red, int green, int blue) {
            // normalize red, green and blue values
            float r = ((float)red / 255.0f);
            float g = ((float)green / 255.0f);
            float b = ((float)blue / 255.0f);

            // conversion start
            float max = Mathf.Max(r, Mathf.Max(g, b));
            float min = Mathf.Min(r, Mathf.Min(g, b));

            float h = 0.0f;
            if(max == r && g >= b) {
                h = 60 * (g - b) / (max - min);
            } else if(max == r && g < b) {
                h = 60 * (g - b) / (max - min) + 360;
            } else if(max == g) {
                h = 60 * (b - r) / (max - min) + 120;
            } else if(max == b) {
                h = 60 * (r - g) / (max - min) + 240;
            }

            float s = (max == 0) ? 0.0f : (1.0f - (min / max));

            return new HSB(h, s, (float)max);
        }

        public override string ToString() {
            return "H: " + (int)hue + " S: " + (int)saturation + " B: " + (int)brightness;
        }

        public static void Test() {
            Debug.Log(Color.red);
            Debug.Log(HSB.ToHSBColor(Color.red));
        }
    }

    /// <summary>
    /// Structure to define HSLColor.
    /// </summary>
    public struct HSLColor {
        /// <summary>
        /// Gets an empty HSLColor structure;
        /// </summary>
        public static readonly HSLColor Empty = new HSLColor();

        private float hue;
        private float saturation;
        private float luminance;

        public static bool operator ==(HSLColor item1, HSLColor item2) {
            return (
                item1.Hue == item2.Hue
                && item1.Saturation == item2.Saturation
                && item1.Luminance == item2.Luminance
                );
        }

        public static bool operator !=(HSLColor item1, HSLColor item2) {
            return (
                item1.Hue != item2.Hue
                || item1.Saturation != item2.Saturation
                || item1.Luminance != item2.Luminance
                );
        }

        /// <summary>
        /// Gets or sets the hue component.
        /// </summary>
        public float Hue {
            get {
                return hue;
            }
            set {
                hue = (value > 360) ? 360 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets saturation component.
        /// </summary>
        public float Saturation {
            get {
                return saturation;
            }
            set {
                saturation = (value > 1) ? 1 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets the luminance component.
        /// </summary>
        public float Luminance {
            get {
                return luminance;
            }
            set {
                luminance = (value > 1) ? 1 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Creates an instance of a HSLColor structure.
        /// </summary>
        /// <param name="h">Hue value.</param>
        /// <param name="s">Saturation value.</param>
        /// <param name="l">Lightness value.</param>
        public HSLColor(float h, float s, float l) {
            this.hue = (h > 360) ? 360 : ((h < 0) ? 0 : h);
            this.saturation = (s > 1) ? 1 : ((s < 0) ? 0 : s);
            this.luminance = (l > 1) ? 1 : ((l < 0) ? 0 : l);
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (HSLColor)obj);
        }

        public override int GetHashCode() {
            return Hue.GetHashCode() ^ Saturation.GetHashCode() ^
                Luminance.GetHashCode();
        }
    }

    /// <summary>
    /// Structure to define CMYKColor.
    /// </summary>
    public struct CMYKColor {
        /// <summary>
        /// Gets an empty CMYKColor structure;
        /// </summary>
        public readonly static CMYKColor Empty = new CMYKColor();

        private float c;
        private float m;
        private float y;
        private float k;

        public static bool operator ==(CMYKColor item1, CMYKColor item2) {
            return (
                item1.Cyan == item2.Cyan
                && item1.Magenta == item2.Magenta
                && item1.Yellow == item2.Yellow
                && item1.Black == item2.Black
                );
        }

        public static bool operator !=(CMYKColor item1, CMYKColor item2) {
            return (
                item1.Cyan != item2.Cyan
                || item1.Magenta != item2.Magenta
                || item1.Yellow != item2.Yellow
                || item1.Black != item2.Black
                );
        }

        public float Cyan {
            get {
                return c;
            }
            set {
                c = value;
                c = (c > 1) ? 1 : ((c < 0) ? 0 : c);
            }
        }

        public float Magenta {
            get {
                return m;
            }
            set {
                m = value;
                m = (m > 1) ? 1 : ((m < 0) ? 0 : m);
            }
        }

        public float Yellow {
            get {
                return y;
            }
            set {
                y = value;
                y = (y > 1) ? 1 : ((y < 0) ? 0 : y);
            }
        }

        public float Black {
            get {
                return k;
            }
            set {
                k = value;
                k = (k > 1) ? 1 : ((k < 0) ? 0 : k);
            }
        }

        /// <summary>
        /// Creates an instance of a CMYKColor structure.
        /// </summary>
        public CMYKColor(float c, float m, float y, float k) {
            this.c = c;
            this.m = m;
            this.y = y;
            this.k = k;
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (CMYKColor)obj);
        }

        public override int GetHashCode() {
            return Cyan.GetHashCode() ^
              Magenta.GetHashCode() ^ Yellow.GetHashCode() ^ Black.GetHashCode();
        }

    }

    /// <summary>
    /// Structure to define YUVColor.
    /// </summary>
    public struct YUVColor {
        /// <summary>
        /// Gets an empty YUVColor structure.
        /// </summary>
        public static readonly YUVColor Empty = new YUVColor();

        private float y;
        private float u;
        private float v;

        public static bool operator ==(YUVColor item1, YUVColor item2) {
            return (
                item1.Y == item2.Y
                && item1.U == item2.U
                && item1.V == item2.V
                );
        }

        public static bool operator !=(YUVColor item1, YUVColor item2) {
            return (
                item1.Y != item2.Y
                || item1.U != item2.U
                || item1.V != item2.V
                );
        }

        public float Y {
            get {
                return y;
            }
            set {
                y = value;
                y = (y > 1) ? 1 : ((y < 0) ? 0 : y);
            }
        }

        public float U {
            get {
                return u;
            }
            set {
                u = value;
                u = (u > 0.436f) ? 0.436f : ((u < -0.436f) ? -0.436f : u);
            }
        }

        public float V {
            get {
                return v;
            }
            set {
                v = value;
                v = (v > 0.615f) ? 0.615f : ((v < -0.615f) ? -0.615f : v);
            }
        }

        /// <summary>
        /// Creates an instance of a YUVColor structure.
        /// </summary>
        public YUVColor(float y, float u, float v) {
            this.y = (y > 1) ? 1 : ((y < 0) ? 0 : y);
            this.u = (u > 0.436f) ? 0.436f : ((u < -0.436f) ? -0.436f : u);
            this.v = (v > 0.615f) ? 0.615f : ((v < -0.615f) ? -0.615f : v);
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (YUVColor)obj);
        }

        public override int GetHashCode() {
            return Y.GetHashCode() ^ U.GetHashCode() ^ V.GetHashCode();
        }

    }

    /// <summary>
    /// Structure to define CIE XYZ.
    /// </summary>
    public struct CIEXYZColor {
        /// <summary>
        /// Gets an empty CIEXYZColor structure.
        /// </summary>
        public static readonly CIEXYZColor Empty = new CIEXYZColor();
        /// <summary>
        /// Gets the CIE D65 (white) structure.
        /// </summary>
        public static readonly CIEXYZColor D65 = new CIEXYZColor(0.9505f, 1.0f, 1.0890f);


        private float x;
        private float y;
        private float z;

        public static bool operator ==(CIEXYZColor item1, CIEXYZColor item2) {
            return (
                item1.X == item2.X
                && item1.Y == item2.Y
                && item1.Z == item2.Z
                );
        }

        public static bool operator !=(CIEXYZColor item1, CIEXYZColor item2) {
            return (
                item1.X != item2.X
                || item1.Y != item2.Y
                || item1.Z != item2.Z
                );
        }

        /// <summary>
        /// Gets or sets X component.
        /// </summary>
        public float X {
            get {
                return this.x;
            }
            set {
                this.x = (value > 0.9505f) ? 0.9505f : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Y component.
        /// </summary>
        public float Y {
            get {
                return this.y;
            }
            set {
                this.y = (value > 1.0f) ? 1.0f : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Z component.
        /// </summary>
        public float Z {
            get {
                return this.z;
            }
            set {
                this.z = (value > 1.089f) ? 1.089f : ((value < 0) ? 0 : value);
            }
        }

        public CIEXYZColor(float x, float y, float z) {
            this.x = (x > 0.9505f) ? 0.9505f : ((x < 0) ? 0 : x);
            this.y = (y > 1.0f) ? 1.0f : ((y < 0) ? 0 : y);
            this.z = (z > 1.089f) ? 1.089f : ((z < 0) ? 0 : z);
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIEXYZColor)obj);
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

    }

    /// <summary>
    /// Structure to define CIE L*a*b*.
    /// </summary>
    public struct CIELabColor {
        /// <summary>
        /// Gets an empty CIELabColor structure.
        /// </summary>
        public static readonly CIELabColor Empty = new CIELabColor();

        private float l;
        private float a;
        private float b;


        public static bool operator ==(CIELabColor item1, CIELabColor item2) {
            return (
                item1.L == item2.L
                && item1.A == item2.A
                && item1.B == item2.B
                );
        }

        public static bool operator !=(CIELabColor item1, CIELabColor item2) {
            return (
                item1.L != item2.L
                || item1.A != item2.A
                || item1.B != item2.B
                );
        }


        /// <summary>
        /// Gets or sets L component.
        /// </summary>
        public float L {
            get {
                return this.l;
            }
            set {
                this.l = value;
            }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public float A {
            get {
                return this.a;
            }
            set {
                this.a = value;
            }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public float B {
            get {
                return this.b;
            }
            set {
                this.b = value;
            }
        }

        public CIELabColor(float l, float a, float b) {
            this.l = l;
            this.a = a;
            this.b = b;
        }

        public override bool Equals(System.Object obj) {
            if(obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIELabColor)obj);
        }

        public override int GetHashCode() {
            return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        }

    }
}
