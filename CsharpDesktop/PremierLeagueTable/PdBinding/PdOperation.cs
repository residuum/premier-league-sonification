using System;
using LibPDBinding;
using PremierLeagueTable.WebData;

namespace PremierLeagueTable.PdBinding
{
    class PdOperation : IDisposable
    {
        readonly int _patchHandle;
        static readonly string DoneReceiver = "done";

        public PdOperation(string filePath)
        {
            _patchHandle = LibPD.OpenPatch(filePath);
            LibPD.Bang += LibPD_Bang;
            LibPD.Subscribe(DoneReceiver);
            LibPD.OpenAudio(0, 2, 44100);
            LibPD.ComputeAudio(true);
        }

        public int BlockSize
        {
            get { return LibPD.BlockSize; }
        }

        public delegate void TeamDone(object sender, EventArgs args);

        public event TeamDone GetNext;

        public void SetOutput(float[] output, int ticks)
        {
            while (LibPD.Process(ticks, new float[0], output) == 0)
            {
                if (BufferReady != null)
                {
                    BufferReady(this, new BufferReadyEventArgs(output));
                }
            }
        }

        public delegate void Processed(object sender, BufferReadyEventArgs args);
        public event Processed BufferReady;

        void LibPD_Bang(string recv)
        {
            if (recv != DoneReceiver) return;
            if (GetNext == null) return;
            GetNext(this, new EventArgs());
        }

        ~PdOperation()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetMaxPoints(int maxPoints)
        {
            LibPD.SendFloat("max_points", maxPoints);
        }

        public void SetCurrentTeam(Team team)
        {
            LibPD.SendList("data", team.ToPdArgs());
        }

        void Dispose(bool isDisposing)
        {
            LibPD.Unsubscribe(DoneReceiver);
            LibPD.ComputeAudio(false);
            if (_patchHandle > 0)
            {
                LibPD.ClosePatch(_patchHandle);
            }
        }
    }
}
