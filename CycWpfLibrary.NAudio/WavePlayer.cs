using NAudio.Wave;

namespace CycWpfLibrary.NAudio
{
  public class WavePlayer
  {
    private WaveFileReader Reader;
    public WaveChannel32 Channel { get; set; }
    public bool IsLooping { get; set; }
    private string FileName { get; set; }

    public WavePlayer()
    {

    }
    public WavePlayer(string FileName, bool IsLooping = false) : this()
    {
      this.FileName = FileName;
      this.IsLooping = IsLooping;
      Initialise();
    }
    private void Initialise()
    {
      Reader = new WaveFileReader(FileName);
      var loop = new LoopStream(Reader) { IsLooping = IsLooping };
      Channel = new WaveChannel32(loop) { PadWithZeroes = false };
    }

    public void Dispose()
    {
      if (Channel != null)
      {
        Channel.Dispose();
        Reader.Dispose();
      }
    }

  }
}
