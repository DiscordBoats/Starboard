namespace Starboat.Util
{
    public struct Version
    {
        private int Major { get; }
        private int Minor { get; }
        private int Revision { get; }

        public int GetMajor() => Major;
        public int GetMinor() => Minor;
        public int GetRev() => Revision;
        override public string ToString() => $"v{Major}.{Minor}.{Revision}";

        public Version(int major, int minor, int rev)
        {
            Major = major;
            Minor = minor;
            Revision = rev;
        }
    }
}