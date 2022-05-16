using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
    public class PostMachineService : IPostMachineService
    {
        private const int MIN_CODE_VALUE_INCL = 100_000;
        private const int MAX_CODE_VALUE_EXCL = 1_000_000;
        private static readonly Random rand = new();

        // Generate unlock code. Unlock code is unique in this post machine.
        // The supplied post machine must have its `ShipmentsWithThisSource`
        // and `ShipmentsWithThisDestination` properties populated
        public int GeneratePostMachineUnlockCode(PostMachine p)
        {
            List<int> codes = new();
            foreach (Shipment s in p.ShipmentsWithThisSource.Concat(p.ShipmentsWithThisDestination))
            {
                if (s.SrcPmSenderUnlockCode != null)
                {
                    codes.Add(s.SrcPmSenderUnlockCode.Value);
                }

                if (s.SrcPmCourierUnlockCode != null)
                {
                    codes.Add(s.SrcPmCourierUnlockCode.Value);
                }

                if (s.DestPmCourierUnlockCode != null)
                {
                    codes.Add(s.DestPmCourierUnlockCode.Value);
                }

                if (s.DestPmReceiverUnlockCode != null)
                {
                    codes.Add(s.DestPmReceiverUnlockCode.Value);
                }
            }

            int newCode = rand.Next(MIN_CODE_VALUE_INCL, MAX_CODE_VALUE_EXCL);
            if (!codes.Contains(newCode))
            {
                return newCode;
            }

            while (true)
            {
                newCode = rand.Next(MIN_CODE_VALUE_INCL, MAX_CODE_VALUE_EXCL);
                if (!codes.Contains(newCode))
                {
                    return newCode;
                }
            }
        }
    }
}
