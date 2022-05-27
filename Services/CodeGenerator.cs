using shipping_service.Persistence.Entities;

namespace shipping_service.Services
{
public class CodeGenerator : ICodeGenerator
{
        private const int MIN_CODE_VALUE_INCL = 100_000;
        private const int MAX_CODE_VALUE_EXCL = 1_000_000;
        private static readonly Random Rand = new();
        // Generate unlock code. Unlock code is unique in this post machine.
        // The supplied post machine must have its `ShipmentsWithThisSource`
        // and `ShipmentsWithThisDestination` properties populated

        public int GeneratePostMachineUnlockCode(PostMachine p){
            List<int> codes = new();
            foreach (int?[] cs in p.ShipmentsWithThisSource.Concat(p.ShipmentsWithThisDestination)
                         .Select(s => new[]
                         {
                             s.SrcPmSenderUnlockCode, s.SrcPmCourierUnlockCode, s.DestPmCourierUnlockCode,
                             s.DestPmReceiverUnlockCode
                         }))
            {
                codes.AddRange(from c in cs where c.HasValue select c.Value);
            }

            int newCode = Rand.Next(MIN_CODE_VALUE_INCL, MAX_CODE_VALUE_EXCL);
            if (!codes.Contains(newCode))
            {
                return newCode;
            }

            while (true)
            {
                newCode = Rand.Next(MIN_CODE_VALUE_INCL, MAX_CODE_VALUE_EXCL);
                if (!codes.Contains(newCode))
                {
                    return newCode;
                }
            }

}
}
}
