using System;
using System.Collections.Generic;


namespace Drawing3d
{

    //Copyright (C) 2015 Wolfgang Nagl

    /// <summary>
    /// This class inherits from <see cref="BSplineSurface"/>  uses his <see cref="BezierSurface.ControlPoints"/>
    /// and additionally the <see cref="Weights"/> to evaluate a 3D BSpline. <see cref="BSplineSurface"/> has a degree for
    /// u and v parameters: <see cref="BSplineSurface.UDegree"/> and <see cref="BSplineSurface.VDegree"/>
    /// The default is 2.
    /// <remarks>Usually: the Number of Controlpoints plus the degree plus 1 gives the number of Knots
    /// </remarks>
    /// </summary>
    [Serializable]
    public class NurbsSurface : BSplineSurface
    {
       
        private Double[,] _Weights;
        /// <summary>
        /// Determs values, which could be interpreted as magnetisms to the ControlPoints.
        /// </summary>
       public Double[,] Weights
        {
            get
            {
                if (_Weights == null)
                {
                    _Weights = new double[ControlPoints.GetLength(0), ControlPoints.GetLength(1)];
                    for (int i = 0; i < _Weights.GetLength(0); i++)
                    {
                        for (int j = 0; j < _Weights.GetLength(1); j++)
                        {
                            _Weights[i, j] = 1;
                        }
                    }
                }

                return _Weights;


            }
            set
            {

                _Weights = value;
            }
        }

        /// <summary>
        /// inserts a new <b>VKnot</b>.
        /// </summary>
        /// <param name="newKnot">value of the new knot.</param>
        public void InsertVKnot(double newKnot)
        {
            // Find KnotIndex
            int KnotIndex = -1;
            for (int i = 0; i < VKnots.Length - 1; i++)
            {
                if ((newKnot >= VKnots[i]) && (newKnot < VKnots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }
                if ((newKnot > VKnots[i]) && (newKnot <= VKnots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }

            }
            if ((KnotIndex < 0) || (KnotIndex - VDegree + 1 < 0) || (KnotIndex >= VKnots.Length))
                throw new System.Exception("No Knot found");

            xyz[,] q = new xyz[ControlPoints.GetLength(0), VDegree];
            double[,] w = new double[ControlPoints.GetLength(0), VDegree];

            for (int index = 0; index < ControlPoints.GetLength(0); index++)
            {
                int j = 0;
                for (int i = KnotIndex - VDegree + 1; i <= KnotIndex; i++)
                {
                    double ai = (newKnot - VKnots[i]) / (VKnots[i + VDegree] - VKnots[i]);

                    q[index, j] = ControlPoints[index, i - 1] * (1 - ai) * Weights[index, i - 1] + ControlPoints[index, i] * ai * Weights[index, i];
                    w[index, j] = Weights[index, i - 1] * (1 - ai) + Weights[index, i] * ai;
                    if (w[index, j] != 0)
                        q[index, j] = q[index, j] * (1 / w[index, j]);
                    j++;



                }
            }

            // Einfügen
            // Verschieben der ControlPunkte:
            xyz[,] NewCtrlPoints = new xyz[ControlPoints.GetLength(0), ControlPoints.GetLength(1) + 1];
            double[,] NewWeights = new double[ControlPoints.GetLength(0), ControlPoints.GetLength(1) + 1];
            for (int index = 0; index < ControlPoints.GetLength(0); index++)
                for (int i = 0; i < ControlPoints.GetLength(1); i++)
                {
                    if (i <= KnotIndex - VDegree)
                    {
                        NewCtrlPoints[index, i] = ControlPoints[index, i];
                        NewWeights[index, i] = Weights[index, i];
                        // if (i == KnotIndex - VDegree) NewWeights[index, i+1] = 0; //// <---------- ausbessern
                    }
                    else
                    {
                        NewWeights[index, i + 1] = Weights[index, i];
                        NewCtrlPoints[index, i + 1] = ControlPoints[index, i];
                    }
                }

            // Einfügen neue
            for (int index = 0; index < ControlPoints.GetLength(0); index++)
                for (int i = KnotIndex - VDegree + 1; i < KnotIndex + 1; i++)
                {

                    NewCtrlPoints[index, i] = q[index, i - KnotIndex + VDegree - 1];
                    NewWeights[index, i] = w[index, i - KnotIndex + VDegree - 1];
                }

            double[] NewKnots = new double[VKnots.Length + 1];
            for (int i = 0; i < VKnots.Length; i++)
            {
                if (i < KnotIndex + 1)
                    NewKnots[i] = VKnots[i];
                if (i == KnotIndex + 1)
                {
                    NewKnots[i] = newKnot;
                    NewKnots[i + 1] = VKnots[i];
                }
                if (i > KnotIndex + 1)
                    NewKnots[i + 1] = VKnots[i];
            }
            ControlPoints = NewCtrlPoints;
            VKnots = NewKnots;
            Weights = NewWeights;
        }
        /// <summary>
        /// inserts a new <b>UKnot</b>.
        /// </summary>
        /// <param name="newKnot">value of the new knot.</param>
        /// <summary>
        /// inserts a new <b>UKnot</b>.
        /// </summary>
        public void InsertUKnot(double newKnot)
        {
            // Find KnotIndex
            int KnotIndex = -1;
            for (int i = 0; i < UKnots.Length - 1; i++)
            {
                if ((newKnot >= UKnots[i]) && (newKnot < UKnots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }
                if ((newKnot > UKnots[i]) && (newKnot <= UKnots[i + 1]))
                {
                    KnotIndex = i;
                    break;
                }

            }
            if ((KnotIndex < 0) || (KnotIndex - UDegree + 1 < 0) || (KnotIndex >= UKnots.Length))
                throw new System.Exception("No Knot found");

            xyz[,] q = new xyz[UDegree, ControlPoints.GetLength(1)];
            double[,] w = new double[UDegree, ControlPoints.GetLength(1)];

            for (int index = 0; index < ControlPoints.GetLength(1); index++)
            {
                int j = 0;
                for (int i = KnotIndex - UDegree + 1; i <= KnotIndex; i++)
                {
                    double ai = (newKnot - UKnots[i]) / (UKnots[i + UDegree] - UKnots[i]);

                    q[j, index] = ControlPoints[i - 1, index] * (1 - ai) * Weights[i - 1, index] + ControlPoints[i, index] * ai * Weights[i, index];
                    w[j, index] = Weights[i - 1, index] * (1 - ai) + Weights[i, index] * ai;
                    if (w[j, index] != 0)
                        q[j, index] = q[j, index] * (1 / w[j, index]);
                    j++;



                }
            }

            // Einfügen
            // Verschieben der ControlPunkte:
            xyz[,] NewCtrlPoints = new xyz[ControlPoints.GetLength(0) + 1, ControlPoints.GetLength(1)];
            double[,] NewWeights = new double[ControlPoints.GetLength(0) + 1, ControlPoints.GetLength(1)];
            for (int index = 0; index < ControlPoints.GetLength(1); index++)
                for (int i = 0; i < ControlPoints.GetLength(0); i++)
                {
                    if (i <= KnotIndex - UDegree)
                    {
                        NewCtrlPoints[i, index] = ControlPoints[i, index];
                        NewWeights[i, index] = Weights[i, index];
                        // if (i == KnotIndex - UDegree) NewWeights[index, i+1] = 0; //// <---------- ausbessern
                    }
                    else
                    {
                        NewWeights[i + 1, index] = Weights[i, index];
                        NewCtrlPoints[i + 1, index] = ControlPoints[i, index];
                    }
                }

            // Einfügen neue
            for (int index = 0; index < ControlPoints.GetLength(1); index++)
                for (int i = KnotIndex - UDegree + 1; i < KnotIndex + 1; i++)
                {

                    NewCtrlPoints[i, index] = q[i - KnotIndex + UDegree - 1, index];
                    NewWeights[i, index] = w[i - KnotIndex + UDegree - 1, index];
                }

            double[] NewKnots = new double[UKnots.Length + 1];
            for (int i = 0; i < UKnots.Length; i++)
            {
                if (i < KnotIndex + 1)
                    NewKnots[i] = UKnots[i];
                if (i == KnotIndex + 1)
                {
                    NewKnots[i] = newKnot;
                    NewKnots[i + 1] = UKnots[i];
                }
                if (i > KnotIndex + 1)
                    NewKnots[i + 1] = UKnots[i];
            }
            ControlPoints = NewCtrlPoints;
            UKnots = NewKnots;
            Weights = NewWeights;
        }

        /// <summary>
        /// gets the nurbs a <see cref="INurbs3d"/>
        /// </summary>
        public INurbs3d AsNurb
        {
            get { return (this as INurbs3d); }
            set
            {
                RefreshEnvBox();
                UDegree = value.getUDegree();
                VDegree = value.getVDegree();
                ControlPoints = value.getCtrlPoints();
                UKnots = value.getUKnots();
                VKnots = value.getVKnots();
                Weights = value.getWeights();
    
                CheckPeriodic();
            }
        }
        //ControlPoints.length = Knots.Length - Order;
        //ControlPoint.Lenght = Knonot.Length- Order - 1
        /// <summary>
        /// Calculates the values for a nurbs, at the parameters u and v.
        /// </summary>
        /// <param name="u">Specifies the u parameter.</param>
        /// <param name="v">Specifies the v parameter.</param>
        /// <returns>Value of a nurbs at u,v.</returns>
        public override xyz Value(double u, double v)
        {
            try
            {

           

            
            xyz Result = new xyz(0, 0, 0);
            if ((UKnots == null) || (VKnots == null)) return Result;
            double[] UCoeff = Utils.CoeffSpline(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity, u);
            double[] VCoeff = Utils.CoeffSpline(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, v);
            int UCount = ControlPoints.GetLength(0);
            int VCount = ControlPoints.GetLength(1);
            
            {
                double W1 = 0;
               
                {


                    int VCoeffIndex = 0;
                    int uCoeffIndex = 0;

                    for (int i = 0; i < UCount; i++)
                    {
                        VCoeffIndex = 0;
                        for (int j = 0; j < VCount; j++)
                        {
                            W1 = W1 + Weights[i, j] * UCoeff[uCoeffIndex] * VCoeff[VCoeffIndex];// *VCoeffDerived[j].Value;
                            VCoeffIndex++;
                        }
                        uCoeffIndex++;
                    }
                }
              
                int uCoeffI = 0;
                int VCoeffI = 0;

                for (int i = 0; i < UCount; i++)
                {
                    VCoeffI = 0;

                    for (int k = 0; k < VCount; k++)
                    {
                        Result = Result + ControlPoints[i, k] * (UCoeff[uCoeffI] * VCoeff[VCoeffI]
                        * Weights[i, k] / W1);
                        VCoeffI++;
                    }
                    uCoeffI++;

                }
            }
                return Base.Absolut(Result);
            }
            catch (Exception)
            {

                if ((ControlPoints.GetLength(0) + UDegree + 1) != UKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the UKnots is wrong. It must be UKnots.Length=ControlPoints.GetLength(0) + UDegree + 1");
                else
                if ((ControlPoints.GetLength(1) + VDegree + 1) != VKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the VKnots is wrong. It must be VKnots.Length=ControlPoints.GetLength(1) + VDegree + 1");
                return Base.BaseO;
            }

            
        }
        





        /// <summary>
        /// Calculates the partial uderivation of a nurbs, at the parameters u and v.
        /// </summary>
        /// <param name="u">Specifies the u parameter.</param>
        /// <param name="v">Specifies the v parameter.</param>
        /// <returns>Partial uderivation of a nurbs at u,v.</returns>
        public override xyz uDerivation(double u, double v)
        {
            if ((UKnots == null) || (VKnots == null)) return new xyz(0, 0, 0);
            Utils.SplineExValue[] UCoeffs = Utils.CoeffSplineEx(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity, u);
            double[] VCoeffs = Utils.CoeffSpline(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, v);
            double WDerived = 0;
            double W = 0;
            for (int i = 0; i < UCoeffs.Length; i++)
            {

                for (int j = 0; j < VCoeffs.Length; j++)
                {
                    W = W + Weights[i, j] * UCoeffs[i].Value * VCoeffs[j];
                    WDerived = WDerived + Weights[i, j] * UCoeffs[i].Derivation * VCoeffs[j];
                }
            }

            xyz Result = new xyz();
            for (int i = 0; i < ControlPoints.GetLength(0); i++)
                for (int k = 0; k < ControlPoints.GetLength(1); k++)
                    Result = Result + ControlPoints[i, k] * (Weights[i, k] * VCoeffs[k] * (UCoeffs[i].Derivation * W - WDerived * UCoeffs[i].Value) / (W * W));
            return Base.Absolut(Result) - Base.BaseO;
        }

        /// <summary>
        /// Calculates the partial v derivation of the <see cref="BSplineSurface"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The partial v derivation.</returns>
        public override xyz vDerivation(double u, double v)
        {
            if ((UKnots == null) || (VKnots == null)) return new xyz(0, 0, 0);
            Utils.SplineExValue[] VCoeffs = Utils.CoeffSplineEx(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, v);
            double[] UCoeffs = Utils.CoeffSpline(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity, u);
            double WDerived = 0;
            double W = 0;
            for (int i = 0; i < UCoeffs.Length; i++)
            {
                for (int j = 0; j < VCoeffs.Length; j++)
                {
                    W = W + Weights[i, j] * UCoeffs[i] * VCoeffs[j].Value;
                    WDerived = WDerived + Weights[i, j] * UCoeffs[i] * VCoeffs[j].Derivation;
                }
            }


            xyz Result = new xyz();
            for (int i = 0; i < ControlPoints.GetLength(0); i++)
                for (int k = 0; k < ControlPoints.GetLength(1); k++)
                    Result = Result + ControlPoints[i, k] * (Weights[i, k] * UCoeffs[i] * (VCoeffs[k].Derivation * W - WDerived * VCoeffs[k].Value) / (W * W));
            return Base.Absolut(Result) - Base.BaseO;
        }
   }

}