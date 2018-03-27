﻿namespace Proteogenomics
{
    public class UTR3Prime
        : UTR
    {
        public UTR3Prime(Exon parent, string chromID, string strand, long oneBasedStart, long oneBasedEnd)
            : base(parent, chromID, strand, oneBasedStart, oneBasedEnd)
        {
        }

        public UTR3Prime(UTR3Prime utr)
            : base(utr)
        {
        }

        public override bool is3Prime()
        {
            return true;
        }

        public override bool is5Prime()
        {
            return false;
        }

        private long utrDistance(Variant variant, Transcript tr)
        {
            long cdsEnd = tr.cdsEnd;
            if (cdsEnd < 0)
            {
                return -1;
            }
            if (IsStrandPlus())
            {
                return variant.OneBasedStart - cdsEnd;
            }
            return cdsEnd - variant.OneBasedEnd;
        }

        public override bool VariantEffect(Variant variant, VariantEffects variantEffects)
        {
            if (!Intersects(variant)) { return false; }

            if (variant.Includes(this) && variant.VarType == Variant.VariantType.DEL)
            {
                variantEffects.addEffectType(variant, this, EffectType.UTR_3_DELETED); // A UTR was removed entirely
                return true;
            }

            Transcript tr = (Transcript)FindParent(typeof(Transcript));
            long distance = utrDistance(variant, tr);

            VariantEffect variantEffect = new VariantEffect(variant);
            variantEffect.set(this, EffectType.UTR_3_PRIME, Proteogenomics.VariantEffect.EffectDictionary[EffectType.UTR_3_PRIME], distance >= 0 ? distance + " bases from CDS" : "");
            variantEffect.setDistance(distance);
            variantEffects.add(variantEffect);

            return true;
        }
    }
}