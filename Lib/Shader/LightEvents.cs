//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Drawing3d

//{
//    partial class GLShader : OGLShader
//    {      float[] toFloat(xyz value)
//        {
//            float[] result = new float[] { (float)value.x, (float)value.y, (float)value.z, (float)1 };
//            return result;
//        }
//        void MakeLightEvents(OpenGlDevice Device)
//        {

//                Field C = getvar("Lights[0].ambient");
//                if (C!=null) C.DoUpdate += ambient_0_Update;
//            C = getvar("Lights[0].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_0_Update;
//            C = getvar("Lights[0].linearAttenuation");
//                if (C != null) C.DoUpdate += linearAttenuation_0_Update;
//                C = getvar("Lights[0].quadraticAttenuation");
//                if (C != null) C.DoUpdate += quadraticAttenuationt_0_Update;
//                C = getvar("Lights[0].diffuse");
//               if (C != null) C.DoUpdate += diffuse_0_Update;
//                C = getvar("Lights[0].specular");
//                if (C != null) C.DoUpdate += Specular_0_Update;
//                 C = getvar("Lights[0].position");
//                if (C != null) C.DoUpdate += position_0_Update;
//                 C = getvar("Lights[0].enabled");
//                if (C != null) C.DoUpdate += enabled_0_Update;

//            C = getvar("Lights[1].ambient");
//            if (C != null) C.DoUpdate += ambient_1_Update;
//            C = getvar("Lights[1].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_1_Update;
//            C = getvar("Lights[1].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_1_Update;
//            C = getvar("Lights[1].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_1_Update;
//            C = getvar("Lights[1].diffuse");
//            if (C != null) C.DoUpdate += diffuse_1_Update;
//            C = getvar("Lights[1].specular");
//            if (C != null) C.DoUpdate += Specular_1_Update;
//            C = getvar("Lights[1].position");
//            if (C != null) C.DoUpdate += position_1_Update;
//            C = getvar("Lights[1].enabled");
//            if (C != null) C.DoUpdate += enabled_1_Update;



//            C = getvar("Lights[2].ambient");
//            if (C != null) C.DoUpdate += ambient_2_Update;
//            C = getvar("Lights[2].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_2_Update;
//            C = getvar("Lights[2].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_2_Update;
//            C = getvar("Lights[2].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_2_Update;
//            C = getvar("Lights[2].diffuse");
//            if (C != null) C.DoUpdate += diffuse_2_Update;
//            C = getvar("Lights[2].specular");
//            if (C != null) C.DoUpdate += Specular_2_Update;
//            C = getvar("Lights[2].position");
//            if (C != null) C.DoUpdate += position_2_Update;
//            C = getvar("Lights[2].enabled");
//            if (C != null) C.DoUpdate += enabled_2_Update;


//            C = getvar("Lights[3].ambient");
//            if (C != null) C.DoUpdate += ambient_3_Update;
//            C = getvar("Lights[3].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_3_Update;
//            C = getvar("Lights[3].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_3_Update;
//            C = getvar("Lights[3].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_3_Update;
//            C = getvar("Lights[3].diffuse");
//            if (C != null) C.DoUpdate += diffuse_3_Update;
//            C = getvar("Lights[3].specular");
//            if (C != null) C.DoUpdate += Specular_3_Update;
//            C = getvar("Lights[3].position");
//            if (C != null) C.DoUpdate += position_3_Update;
//            C = getvar("Lights[3].enabled");
//            if (C != null) C.DoUpdate += enabled_3_Update;


//            C = getvar("Lights[4].ambient");
//            if (C != null) C.DoUpdate += ambient_4_Update;
//            C = getvar("Lights[4].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_4_Update;
//            C = getvar("Lights[4].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_4_Update;
//            C = getvar("Lights[4].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_4_Update;
//            C = getvar("Lights[4].diffuse");
//            if (C != null) C.DoUpdate += diffuse_4_Update;
//            C = getvar("Lights[4].specular");
//            if (C != null) C.DoUpdate += Specular_4_Update;
//            C = getvar("Lights[4].position");
//            if (C != null) C.DoUpdate += position_4_Update;
//            C = getvar("Lights[4].enabled");
//            if (C != null) C.DoUpdate += enabled_4_Update;


//            C = getvar("Lights[5].ambient");
//            if (C != null) C.DoUpdate += ambient_5_Update;
//            C = getvar("Lights[5].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_5_Update;
//            C = getvar("Lights[5].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_5_Update;
//            C = getvar("Lights[5].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_5_Update;
//            C = getvar("Lights[5].diffuse");
//            if (C != null) C.DoUpdate += diffuse_5_Update;
//            C = getvar("Lights[5].specular");
//            if (C != null) C.DoUpdate += Specular_5_Update;
//            C = getvar("Lights[5].position");
//            if (C != null) C.DoUpdate += position_5_Update;
//            C = getvar("Lights[5].enabled");
//            if (C != null) C.DoUpdate += enabled_5_Update;


//            C = getvar("Lights[6].ambient");
//            if (C != null) C.DoUpdate += ambient_6_Update;
//            C = getvar("Lights[6].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_6_Update;
//            C = getvar("Lights[6].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_6_Update;
//            C = getvar("Lights[6].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_6_Update;
//            C = getvar("Lights[6].diffuse");
//            if (C != null) C.DoUpdate += diffuse_6_Update;
//            C = getvar("Lights[6].specular");
//            if (C != null) C.DoUpdate += Specular_6_Update;
//            C = getvar("Lights[6].position");
//            if (C != null) C.DoUpdate += position_6_Update;
//            C = getvar("Lights[6].enabled");
//            if (C != null) C.DoUpdate += enabled_6_Update;


//            C = getvar("Lights[7].ambient");
//            if (C != null) C.DoUpdate += ambient_7_Update;
//            C = getvar("Lights[7].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_7_Update;
//            C = getvar("Lights[7].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_7_Update;
//            C = getvar("Lights[7].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_7_Update;
//            C = getvar("Lights[7].diffuse");
//            if (C != null) C.DoUpdate += diffuse_7_Update;
//            C = getvar("Lights[7].specular");
//            if (C != null) C.DoUpdate += Specular_7_Update;
//            C = getvar("Lights[7].position");
//            if (C != null) C.DoUpdate += position_7_Update;
//            C = getvar("Lights[7].enabled");
//            if (C != null) C.DoUpdate += enabled_7_Update;


//            C = getvar("Lights[8].ambient");
//            if (C != null) C.DoUpdate += ambient_8_Update;
//            C = getvar("Lights[8].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_8_Update;
//            C = getvar("Lights[8].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_8_Update;
//            C = getvar("Lights[8].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_8_Update;
//            C = getvar("Lights[8].diffuse");
//            if (C != null) C.DoUpdate += diffuse_8_Update;
//            C = getvar("Lights[8].specular");
//            if (C != null) C.DoUpdate += Specular_8_Update;
//            C = getvar("Lights[8].position");
//            if (C != null) C.DoUpdate += position_8_Update;
//            C = getvar("Lights[8].enabled");
//            if (C != null) C.DoUpdate += enabled_8_Update;



//            C = getvar("Lights[9].ambient");
//            if (C != null) C.DoUpdate += ambient_9_Update;
//            C = getvar("Lights[9].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_9_Update;
//            C = getvar("Lights[9].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_9_Update;
//            C = getvar("Lights[9].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_9_Update;
//            C = getvar("Lights[9].diffuse");
//            if (C != null) C.DoUpdate += diffuse_9_Update;
//            C = getvar("Lights[9].specular");
//            if (C != null) C.DoUpdate += Specular_9_Update;
//            C = getvar("Lights[9].position");
//            if (C != null) C.DoUpdate += position_9_Update;
//            C = getvar("Lights[9].enabled");
//            if (C != null) C.DoUpdate += enabled_9_Update;



//            C = getvar("Lights[10].ambient");
//            if (C != null) C.DoUpdate += ambient_10_Update;
//            C = getvar("Lights[10].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_10_Update;
//            C = getvar("Lights[10].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_10_Update;
//            C = getvar("Lights[10].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_10_Update;
//            C = getvar("Lights[10].diffuse");
//            if (C != null) C.DoUpdate += diffuse_10_Update;
//            C = getvar("Lights[10].specular");
//            if (C != null) C.DoUpdate += Specular_10_Update;
//            C = getvar("Lights[10].position");
//            if (C != null) C.DoUpdate += position_10_Update;
//            C = getvar("Lights[10].enabled");
//            if (C != null) C.DoUpdate += enabled_10_Update;



//            C = getvar("Lights[11].ambient");
//            if (C != null) C.DoUpdate += ambient_11_Update;
//            C = getvar("Lights[11].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_11_Update;
//            C = getvar("Lights[11].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_11_Update;
//            C = getvar("Lights[11].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_11_Update;
//            C = getvar("Lights[11].diffuse");
//            if (C != null) C.DoUpdate += diffuse_11_Update;
//            C = getvar("Lights[11].specular");
//            if (C != null) C.DoUpdate += Specular_11_Update;
//            C = getvar("Lights[11].position");
//            if (C != null) C.DoUpdate += position_11_Update;
//            C = getvar("Lights[11].enabled");
//            if (C != null) C.DoUpdate += enabled_11_Update;


//            C = getvar("Lights[12].ambient");
//            if (C != null) C.DoUpdate += ambient_12_Update;
//            C = getvar("Lights[12].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_12_Update;
//            C = getvar("Lights[12].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_12_Update;
//            C = getvar("Lights[12].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_12_Update;
//            C = getvar("Lights[12].diffuse");
//            if (C != null) C.DoUpdate += diffuse_12_Update;
//            C = getvar("Lights[12].specular");
//            if (C != null) C.DoUpdate += Specular_12_Update;
//            C = getvar("Lights[12].position");
//            if (C != null) C.DoUpdate += position_12_Update;
//            C = getvar("Lights[12].enabled");
//            if (C != null) C.DoUpdate += enabled_12_Update;


//            C = getvar("Lights[13].ambient");
//            if (C != null) C.DoUpdate += ambient_13_Update;
//            C = getvar("Lights[13].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_13_Update;
//            C = getvar("Lights[13].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_13_Update;
//            C = getvar("Lights[13].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_13_Update;
//            C = getvar("Lights[13].diffuse");
//            if (C != null) C.DoUpdate += diffuse_13_Update;
//            C = getvar("Lights[13].specular");
//            if (C != null) C.DoUpdate += Specular_13_Update;
//            C = getvar("Lights[13].position");
//            if (C != null) C.DoUpdate += position_13_Update;
//            C = getvar("Lights[13].enabled");
//            if (C != null) C.DoUpdate += enabled_13_Update;



//            C = getvar("Lights[14].ambient");
//            if (C != null) C.DoUpdate += ambient_14_Update;
//            C = getvar("Lights[14].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_14_Update;
//            C = getvar("Lights[14].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_14_Update;
//            C = getvar("Lights[14].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_14_Update;
//            C = getvar("Lights[14].diffuse");
//            if (C != null) C.DoUpdate += diffuse_14_Update;
//            C = getvar("Lights[14].specular");
//            if (C != null) C.DoUpdate += Specular_14_Update;
//            C = getvar("Lights[14].position");
//            if (C != null) C.DoUpdate += position_14_Update;
//            C = getvar("Lights[14].enabled");
//            if (C != null) C.DoUpdate += enabled_14_Update;



//            C = getvar("Lights[15].ambient");
//            if (C != null) C.DoUpdate += ambient_15_Update;
//            C = getvar("Lights[15].constantAttenuation");
//            if (C != null) C.DoUpdate += constantAttenuation_15_Update;
//            C = getvar("Lights[15].linearAttenuation");
//            if (C != null) C.DoUpdate += linearAttenuation_15_Update;
//            C = getvar("Lights[15].quadraticAttenuation");
//            if (C != null) C.DoUpdate += quadraticAttenuationt_15_Update;
//            C = getvar("Lights[15].diffuse");
//            if (C != null) C.DoUpdate += diffuse_15_Update;
//            C = getvar("Lights[15].specular");
//            if (C != null) C.DoUpdate += Specular_15_Update;
//            C = getvar("Lights[15].position");
//            if (C != null) C.DoUpdate += position_15_Update;
//            C = getvar("Lights[15].enabled");
//            if (C != null) C.DoUpdate += enabled_15_Update;









//        }
//        private void constantAttenuation_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].ConstantAttenuation);
//        }
//        private void constantAttenuation_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].ConstantAttenuation);
//        }
//        private void constantAttenuation_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].ConstantAttenuation);
//        }
//        private void constantAttenuation_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].ConstantAttenuation);
//        }
//        private void constantAttenuation_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].ConstantAttenuation);
//        }
//        private void constantAttenuation_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].ConstantAttenuation);
//        }
//        private void constantAttenuation_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].ConstantAttenuation);
//        }
//        private void constantAttenuation_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].ConstantAttenuation);
//        }
//        private void constantAttenuation_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].ConstantAttenuation);
//        }
//        private void constantAttenuation_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].ConstantAttenuation);
//        }
//        private void constantAttenuation_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].ConstantAttenuation);
//        }
//        private void constantAttenuation_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].ConstantAttenuation);
//        }
//        private void constantAttenuation_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].ConstantAttenuation);
//        }
//        private void constantAttenuation_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].ConstantAttenuation);
//        }
//        private void constantAttenuation_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].ConstantAttenuation);
//        }
//        private void constantAttenuation_15_Update(Field sender, object UpdateObject)
//        {   
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].ConstantAttenuation);
//        }
//        private void ambient_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].Ambient);
//        }
//        private void linearAttenuation_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].QuadraticAttenuation);
//        }
//        private void diffuse_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].Diffuse);
//        }
//        private void Specular_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[0].Specular);
//        }
//        private void position_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat( Device.Lights[0].Position));
//        }
//        private void enabled_0_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;

//            sender.SetValue(Device.Lights[0].Enable);
//        }


//        private void ambient_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].Ambient);
//        }
//        private void linearAttenuation_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].QuadraticAttenuation);
//        }
//        private void diffuse_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].Diffuse);
//        }
//        private void Specular_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].Specular);
//        }
//        private void position_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[1].Position));
//        }   
//        private void enabled_1_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[1].Enable);
//        }



//        private void ambient_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].Ambient);
//        }
//        private void linearAttenuation_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].QuadraticAttenuation);
//        }
//        private void diffuse_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].Diffuse);
//        }
//        private void Specular_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].Specular);
//        }
//        private void position_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[2].Position));
//        }
//        private void enabled_2_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[2].Enable);
//        }



//        private void ambient_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].Ambient);
//        }
//        private void linearAttenuation_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].QuadraticAttenuation);
//        }
//        private void diffuse_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].Diffuse);
//        }
//        private void Specular_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].Specular);
//        }
//        private void position_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[3].Position));
//        }
//        private void enabled_3_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[3].Enable);
//        }



//        private void ambient_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].Ambient);
//        }
//        private void linearAttenuation_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].QuadraticAttenuation);
//        }
//        private void diffuse_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].Diffuse);
//        }
//        private void Specular_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].Specular);
//        }
//        private void position_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[4].Position));
//        }
//        private void enabled_4_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[4].Enable);
//        }



//        private void ambient_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].Ambient);
//        }
//        private void linearAttenuation_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].QuadraticAttenuation);
//        }
//        private void diffuse_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].Diffuse);
//        }
//        private void Specular_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].Specular);
//        }
//        private void position_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[5].Position));
//        }
//        private void enabled_5_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[5].Enable);
//        }


//        private void ambient_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].Ambient);
//        }
//        private void linearAttenuation_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].QuadraticAttenuation);
//        }
//        private void diffuse_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].Diffuse);
//        }
//        private void Specular_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].Specular);
//        }
//        private void position_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[6].Position));
//        }
//        private void enabled_6_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[6].Enable);
//        }


//        private void ambient_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].Ambient);
//        }
//        private void linearAttenuation_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].QuadraticAttenuation);
//        }
//        private void diffuse_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].Diffuse);
//        }
//        private void Specular_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].Specular);
//        }
//        private void position_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[7].Position));
//        }
//        private void enabled_7_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[7].Enable);
//        }


//        private void ambient_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].Ambient);
//        }
//        private void linearAttenuation_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].QuadraticAttenuation);
//        }
//        private void diffuse_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].Diffuse);
//        }
//        private void Specular_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].Specular);
//        }
//        private void position_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[8].Position));
//        }
//        private void enabled_8_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[8].Enable);
//        }


//        private void ambient_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].Ambient);
//        }
//        private void linearAttenuation_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].QuadraticAttenuation);
//        }
//        private void diffuse_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].Diffuse);
//        }
//        private void Specular_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].Specular);
//        }
//        private void position_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[9].Position));
//        }
//        private void enabled_9_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[9].Enable);
//        }


//        private void ambient_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].Ambient);
//        }
//        private void linearAttenuation_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].QuadraticAttenuation);
//        }
//        private void diffuse_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].Diffuse);
//        }
//        private void Specular_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].Specular);
//        }
//        private void position_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[10].Position));
//        }
//        private void enabled_10_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[10].Enable);
//        }


//        private void ambient_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].Ambient);
//        }
//        private void linearAttenuation_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].QuadraticAttenuation);
//        }
//        private void diffuse_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].Diffuse);
//        }
//        private void Specular_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].Specular);
//        }
//        private void position_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[11].Position));
//        }
//        private void enabled_11_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[11].Enable);
//        }



//        private void ambient_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].Ambient);
//        }
//        private void linearAttenuation_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].QuadraticAttenuation);
//        }
//        private void diffuse_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].Diffuse);
//        }
//        private void Specular_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].Specular);
//        }
//        private void position_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[12].Position));
//        }
//        private void enabled_12_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[12].Enable);
//        }



//        private void ambient_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].Ambient);
//        }
//        private void linearAttenuation_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].QuadraticAttenuation);
//        }
//        private void diffuse_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].Diffuse);
//        }
//        private void Specular_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].Specular);
//        }
//        private void position_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[13].Position));
//        }
//        private void enabled_13_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[13].Enable);
//        }



//        private void ambient_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].Ambient);
//        }
//        private void linearAttenuation_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].QuadraticAttenuation);
//        }
//        private void diffuse_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].Diffuse);
//        }
//        private void Specular_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].Specular);
//        }
//        private void position_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[14].Position));
//        }
//        private void enabled_14_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].Enable);
//        }
//        private void ambient_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[14].Ambient);
//        }
//        private void linearAttenuation_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].LinearAttenuation);
//        }
//        private void quadraticAttenuationt_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].QuadraticAttenuation);
//        }
//        private void diffuse_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].Diffuse);
//        }
//        private void Specular_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].Specular);
//        }
//        private void position_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(toFloat(Device.Lights[15].Position));
//        }
//        private void enabled_15_Update(Field sender, object UpdateObject)
//        {
//            OpenGlDevice Device = UpdateObject as OpenGlDevice;
//            sender.SetValue(Device.Lights[15].Enable);
//        }

//    }
//}
using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d

{
    partial class GLShader : OGLShader
    {
        float[] toFloat(xyzwf value)
        {
            float[] result = new float[] { (float)value.x, (float)value.y, (float)value.z, (float)value.w };
            return result;
        }
        void MakeLightEvents(OpenGlDevice Device)
        {

            Field C = getvar("Lights[0].ambient");
            if (C != null) C.DoUpdate += ambient_0_Update;
            C = getvar("Lights[0].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_0_Update;
            C = getvar("Lights[0].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_0_Update;
            C = getvar("Lights[0].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_0_Update;
            C = getvar("Lights[0].diffuse");
            if (C != null) C.DoUpdate += diffuse_0_Update;
            C = getvar("Lights[0].specular");
            if (C != null) C.DoUpdate += Specular_0_Update;
            C = getvar("Lights[0].position");
            if (C != null) C.DoUpdate += position_0_Update;
            C = getvar("Lights[0].enabled");
            if (C != null) C.DoUpdate += enabled_0_Update;
            C = getvar("Lights[0].specular");
            if (C != null) C.DoUpdate += Specular_0_Update;
            C = getvar("Lights[0].position");
            if (C != null) C.DoUpdate += position_0_Update;
            C = getvar("Lights[0].enabled");
            if (C != null) C.DoUpdate += enabled_0_Update;



            C = getvar("Lights[0].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_0_Update;
            C = getvar("Lights[0].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_0_Update;
            C = getvar("Lights[0].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_0_Update;
            C = getvar("Lights[0].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_0_Update;




            C = getvar("Lights[1].ambient");
            if (C != null) C.DoUpdate += ambient_1_Update;
            C = getvar("Lights[1].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_1_Update;
            C = getvar("Lights[1].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_1_Update;
            C = getvar("Lights[1].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_1_Update;
            C = getvar("Lights[1].diffuse");
            if (C != null) C.DoUpdate += diffuse_1_Update;
            C = getvar("Lights[1].specular");
            if (C != null) C.DoUpdate += Specular_1_Update;
            C = getvar("Lights[1].position");
            if (C != null) C.DoUpdate += position_1_Update;
            C = getvar("Lights[1].enabled");
            if (C != null) C.DoUpdate += enabled_1_Update;

            C = getvar("Lights[1].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_1_Update;
            C = getvar("Lights[1].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_1_Update;
            C = getvar("Lights[1].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_1_Update;
            C = getvar("Lights[1].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_1_Update;

            C = getvar("Lights[2].ambient");
            if (C != null) C.DoUpdate += ambient_2_Update;
            C = getvar("Lights[2].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_2_Update;
            C = getvar("Lights[2].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_2_Update;
            C = getvar("Lights[2].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_2_Update;
            C = getvar("Lights[2].diffuse");
            if (C != null) C.DoUpdate += diffuse_2_Update;
            C = getvar("Lights[2].specular");
            if (C != null) C.DoUpdate += Specular_2_Update;
            C = getvar("Lights[2].position");
            if (C != null) C.DoUpdate += position_2_Update;
            C = getvar("Lights[2].enabled");
            if (C != null) C.DoUpdate += enabled_2_Update;


            C = getvar("Lights[2].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_2_Update;
            C = getvar("Lights[2].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_2_Update;
            C = getvar("Lights[2].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_2_Update;
            C = getvar("Lights[2].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_2_Update;

            C = getvar("Lights[3].ambient");
            if (C != null) C.DoUpdate += ambient_3_Update;
            C = getvar("Lights[3].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_3_Update;
            C = getvar("Lights[3].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_3_Update;
            C = getvar("Lights[3].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_3_Update;
            C = getvar("Lights[3].diffuse");
            if (C != null) C.DoUpdate += diffuse_3_Update;
            C = getvar("Lights[3].specular");
            if (C != null) C.DoUpdate += Specular_3_Update;
            C = getvar("Lights[3].position");
            if (C != null) C.DoUpdate += position_3_Update;
            C = getvar("Lights[3].enabled");
            if (C != null) C.DoUpdate += enabled_3_Update;


            C = getvar("Lights[3].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_3_Update;
            C = getvar("Lights[3].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_3_Update;
            C = getvar("Lights[3].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_3_Update;
            C = getvar("Lights[3].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_3_Update;


            C = getvar("Lights[4].ambient");
            if (C != null) C.DoUpdate += ambient_4_Update;
            C = getvar("Lights[4].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_4_Update;
            C = getvar("Lights[4].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_4_Update;
            C = getvar("Lights[4].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_4_Update;
            C = getvar("Lights[4].diffuse");
            if (C != null) C.DoUpdate += diffuse_4_Update;
            C = getvar("Lights[4].specular");
            if (C != null) C.DoUpdate += Specular_4_Update;
            C = getvar("Lights[4].position");
            if (C != null) C.DoUpdate += position_4_Update;
            C = getvar("Lights[4].enabled");
            if (C != null) C.DoUpdate += enabled_4_Update;



            C = getvar("Lights[4].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_4_Update;
            C = getvar("Lights[4].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_4_Update;
            C = getvar("Lights[4].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_4_Update;
            C = getvar("Lights[4].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_4_Update;

            C = getvar("Lights[5].ambient");
            if (C != null) C.DoUpdate += ambient_5_Update;
            C = getvar("Lights[5].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_5_Update;
            C = getvar("Lights[5].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_5_Update;
            C = getvar("Lights[5].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_5_Update;
            C = getvar("Lights[5].diffuse");
            if (C != null) C.DoUpdate += diffuse_5_Update;
            C = getvar("Lights[5].specular");
            if (C != null) C.DoUpdate += Specular_5_Update;
            C = getvar("Lights[5].position");
            if (C != null) C.DoUpdate += position_5_Update;
            C = getvar("Lights[5].enabled");
            if (C != null) C.DoUpdate += enabled_5_Update;


            C = getvar("Lights[5].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_5_Update;
            C = getvar("Lights[5].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_5_Update;
            C = getvar("Lights[5].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_5_Update;
            C = getvar("Lights[5].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_5_Update;


            C = getvar("Lights[6].ambient");
            if (C != null) C.DoUpdate += ambient_6_Update;
            C = getvar("Lights[6].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_6_Update;
            C = getvar("Lights[6].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_6_Update;
            C = getvar("Lights[6].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_6_Update;
            C = getvar("Lights[6].diffuse");
            if (C != null) C.DoUpdate += diffuse_6_Update;
            C = getvar("Lights[6].specular");
            if (C != null) C.DoUpdate += Specular_6_Update;
            C = getvar("Lights[6].position");
            if (C != null) C.DoUpdate += position_6_Update;
            C = getvar("Lights[6].enabled");
            if (C != null) C.DoUpdate += enabled_6_Update;


            C = getvar("Lights[6].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_6_Update;
            C = getvar("Lights[6].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_6_Update;
            C = getvar("Lights[6].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_6_Update;
            C = getvar("Lights[6].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_6_Update;

            C = getvar("Lights[7].ambient");
            if (C != null) C.DoUpdate += ambient_7_Update;
            C = getvar("Lights[7].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_7_Update;
            C = getvar("Lights[7].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_7_Update;
            C = getvar("Lights[7].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_7_Update;
            C = getvar("Lights[7].diffuse");
            if (C != null) C.DoUpdate += diffuse_7_Update;
            C = getvar("Lights[7].specular");
            if (C != null) C.DoUpdate += Specular_7_Update;
            C = getvar("Lights[7].position");
            if (C != null) C.DoUpdate += position_7_Update;
            C = getvar("Lights[7].enabled");
            if (C != null) C.DoUpdate += enabled_7_Update;


            C = getvar("Lights[7].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_7_Update;
            C = getvar("Lights[7].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_7_Update;
            C = getvar("Lights[7].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_7_Update;
            C = getvar("Lights[7].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_7_Update;

            C = getvar("Lights[8].ambient");
            if (C != null) C.DoUpdate += ambient_8_Update;
            C = getvar("Lights[8].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_8_Update;
            C = getvar("Lights[8].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_8_Update;
            C = getvar("Lights[8].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_8_Update;
            C = getvar("Lights[8].diffuse");
            if (C != null) C.DoUpdate += diffuse_8_Update;
            C = getvar("Lights[8].specular");
            if (C != null) C.DoUpdate += Specular_8_Update;
            C = getvar("Lights[8].position");
            if (C != null) C.DoUpdate += position_8_Update;
            C = getvar("Lights[8].enabled");
            if (C != null) C.DoUpdate += enabled_8_Update;

            C = getvar("Lights[8].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_8_Update;
            C = getvar("Lights[8].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_8_Update;
            C = getvar("Lights[8].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_8_Update;
            C = getvar("Lights[8].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_8_Update;

            C = getvar("Lights[9].ambient");
            if (C != null) C.DoUpdate += ambient_9_Update;
            C = getvar("Lights[9].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_9_Update;
            C = getvar("Lights[9].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_9_Update;
            C = getvar("Lights[9].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_9_Update;
            C = getvar("Lights[9].diffuse");
            if (C != null) C.DoUpdate += diffuse_9_Update;
            C = getvar("Lights[9].specular");
            if (C != null) C.DoUpdate += Specular_9_Update;
            C = getvar("Lights[9].position");
            if (C != null) C.DoUpdate += position_9_Update;
            C = getvar("Lights[9].enabled");
            if (C != null) C.DoUpdate += enabled_9_Update;

            C = getvar("Lights[9].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_9_Update;
            C = getvar("Lights[9].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_9_Update;
            C = getvar("Lights[9].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_9_Update;
            C = getvar("Lights[9].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_9_Update;

            C = getvar("Lights[10].ambient");
            if (C != null) C.DoUpdate += ambient_10_Update;
            C = getvar("Lights[10].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_10_Update;
            C = getvar("Lights[10].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_10_Update;
            C = getvar("Lights[10].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_10_Update;
            C = getvar("Lights[10].diffuse");
            if (C != null) C.DoUpdate += diffuse_10_Update;
            C = getvar("Lights[10].specular");
            if (C != null) C.DoUpdate += Specular_10_Update;
            C = getvar("Lights[10].position");
            if (C != null) C.DoUpdate += position_10_Update;
            C = getvar("Lights[10].enabled");
            if (C != null) C.DoUpdate += enabled_10_Update;

            C = getvar("Lights[10].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_10_Update;
            C = getvar("Lights[10].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_10_Update;
            C = getvar("Lights[10].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_10_Update;
            C = getvar("Lights[10].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_10_Update;

            C = getvar("Lights[11].ambient");
            if (C != null) C.DoUpdate += ambient_11_Update;
            C = getvar("Lights[11].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_11_Update;
            C = getvar("Lights[11].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_11_Update;
            C = getvar("Lights[11].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_11_Update;
            C = getvar("Lights[11].diffuse");
            if (C != null) C.DoUpdate += diffuse_11_Update;
            C = getvar("Lights[11].specular");
            if (C != null) C.DoUpdate += Specular_11_Update;
            C = getvar("Lights[11].position");
            if (C != null) C.DoUpdate += position_11_Update;
            C = getvar("Lights[11].enabled");
            if (C != null) C.DoUpdate += enabled_11_Update;


            C = getvar("Lights[11].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_11_Update;
            C = getvar("Lights[11].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_11_Update;
            C = getvar("Lights[11].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_11_Update;
            C = getvar("Lights[11].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_11_Update;


            C = getvar("Lights[12].ambient");
            if (C != null) C.DoUpdate += ambient_12_Update;
            C = getvar("Lights[12].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_12_Update;
            C = getvar("Lights[12].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_12_Update;
            C = getvar("Lights[12].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_12_Update;
            C = getvar("Lights[12].diffuse");
            if (C != null) C.DoUpdate += diffuse_12_Update;
            C = getvar("Lights[12].specular");
            if (C != null) C.DoUpdate += Specular_12_Update;
            C = getvar("Lights[12].position");
            if (C != null) C.DoUpdate += position_12_Update;
            C = getvar("Lights[12].enabled");
            if (C != null) C.DoUpdate += enabled_12_Update;


            C = getvar("Lights[12].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_12_Update;
            C = getvar("Lights[12].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_12_Update;
            C = getvar("Lights[12].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_12_Update;
            C = getvar("Lights[12].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_12_Update;

            C = getvar("Lights[13].ambient");
            if (C != null) C.DoUpdate += ambient_13_Update;
            C = getvar("Lights[13].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_13_Update;
            C = getvar("Lights[13].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_13_Update;
            C = getvar("Lights[13].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_13_Update;
            C = getvar("Lights[13].diffuse");
            if (C != null) C.DoUpdate += diffuse_13_Update;
            C = getvar("Lights[13].specular");
            if (C != null) C.DoUpdate += Specular_13_Update;
            C = getvar("Lights[13].position");
            if (C != null) C.DoUpdate += position_13_Update;
            C = getvar("Lights[13].enabled");
            if (C != null) C.DoUpdate += enabled_13_Update;


            C = getvar("Lights[13].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_13_Update;
            C = getvar("Lights[13].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_13_Update;
            C = getvar("Lights[13].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_13_Update;
            C = getvar("Lights[13].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_13_Update;


            C = getvar("Lights[14].ambient");
            if (C != null) C.DoUpdate += ambient_14_Update;
            C = getvar("Lights[14].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_14_Update;
            C = getvar("Lights[14].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_14_Update;
            C = getvar("Lights[14].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_14_Update;
            C = getvar("Lights[14].diffuse");
            if (C != null) C.DoUpdate += diffuse_14_Update;
            C = getvar("Lights[14].specular");
            if (C != null) C.DoUpdate += Specular_14_Update;
            C = getvar("Lights[14].position");
            if (C != null) C.DoUpdate += position_14_Update;
            C = getvar("Lights[14].enabled");
            if (C != null) C.DoUpdate += enabled_14_Update;

            C = getvar("Lights[14].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_14_Update;
            C = getvar("Lights[14].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_14_Update;
            C = getvar("Lights[14].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_14_Update;
            C = getvar("Lights[14].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_14_Update;

            C = getvar("Lights[15].ambient");
            if (C != null) C.DoUpdate += ambient_15_Update;
            C = getvar("Lights[15].constantAttenuation");
            if (C != null) C.DoUpdate += constantAttenuation_15_Update;
            C = getvar("Lights[15].linearAttenuation");
            if (C != null) C.DoUpdate += linearAttenuation_15_Update;
            C = getvar("Lights[15].quadraticAttenuation");
            if (C != null) C.DoUpdate += quadraticAttenuationt_15_Update;
            C = getvar("Lights[15].diffuse");
            if (C != null) C.DoUpdate += diffuse_15_Update;
            C = getvar("Lights[15].specular");
            if (C != null) C.DoUpdate += Specular_15_Update;
            C = getvar("Lights[15].position");
            if (C != null) C.DoUpdate += position_15_Update;
            C = getvar("Lights[15].enabled");
            if (C != null) C.DoUpdate += enabled_15_Update;

            C = getvar("Lights[15].spotDirection");
            if (C != null) C.DoUpdate += spotDirection_15_Update;
            C = getvar("Lights[15].spotExponent");
            if (C != null) C.DoUpdate += spotExponent_15_Update;
            C = getvar("Lights[15].spotCutoff");
            if (C != null) C.DoUpdate += spotCutoff_15_Update;
            C = getvar("Lights[15].spotCosCutoff");
            if (C != null) C.DoUpdate += spotCosCutoff_15_Update;







        }

        private void spotDirection_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].spotDirection);
        }
        private void spotExponent_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].spotExponent);
        }

        private void spotCutoff_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].spotCutoff);
        }
        private void spotCosCutoff_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].spotCosCutoff);
        }
        private void spotDirection_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].spotDirection);
        }
        private void spotExponent_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].spotExponent);
        }

        private void spotCutoff_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].spotCutoff);
        }
        private void spotCosCutoff_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].spotCosCutoff);
        }

        private void spotDirection_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].spotDirection);
        }
        private void spotExponent_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].spotExponent);
        }

        private void spotCutoff_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].spotCutoff);
        }
        private void spotCosCutoff_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].spotCosCutoff);
        }
        private void spotDirection_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].spotDirection);
        }
        private void spotExponent_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].spotExponent);
        }

        private void spotCutoff_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].spotCutoff);
        }
        private void spotCosCutoff_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].spotCosCutoff);
        }
        private void spotDirection_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].spotDirection);
        }
        private void spotExponent_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].spotExponent);
        }

        private void spotCutoff_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].spotCutoff);
        }
        private void spotCosCutoff_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].spotCosCutoff);
        }
        private void spotDirection_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].spotDirection);
        }
        private void spotExponent_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].spotExponent);
        }

        private void spotCutoff_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].spotCutoff);
        }
        private void spotCosCutoff_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].spotCosCutoff);
        }

        private void spotDirection_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].spotDirection);
        }
        private void spotExponent_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].spotExponent);
        }

        private void spotCutoff_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].spotCutoff);
        }
        private void spotCosCutoff_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].spotCosCutoff);
        }

        private void spotDirection_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].spotDirection);
        }
        private void spotExponent_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].spotExponent);
        }

        private void spotCutoff_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].spotCutoff);
        }
        private void spotCosCutoff_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].spotCosCutoff);
        }
        private void spotDirection_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].spotDirection);
        }
        private void spotExponent_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].spotExponent);
        }

        private void spotCutoff_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].spotCutoff);
        }
        private void spotCosCutoff_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].spotCosCutoff);
        }
        private void spotDirection_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].spotDirection);
        }
        private void spotExponent_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].spotExponent);
        }

        private void spotCutoff_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].spotCutoff);
        }
        private void spotCosCutoff_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
             sender.SetValue(Device.Lights[9].spotCosCutoff);
        }
        private void spotDirection_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].spotDirection);
        }
        private void spotExponent_10_Update(Field sender, object UpdateObject)
        {
               OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].spotExponent);
        }

        private void spotCutoff_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].spotCutoff);
        }
        private void spotCosCutoff_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].spotCosCutoff);
        }
        private void spotDirection_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].spotDirection);
        }
        private void spotExponent_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].spotExponent);
        }

        private void spotCutoff_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].spotCutoff);
        }
        private void spotCosCutoff_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].spotCosCutoff);
        }
        private void spotDirection_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].spotDirection);
        }
        private void spotExponent_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].spotExponent);
        }

        private void spotCutoff_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].spotCutoff);
        }
        private void spotCosCutoff_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].spotCosCutoff);
        }

        private void spotDirection_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].spotDirection);
        }
        private void spotExponent_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].spotExponent);
        }

        private void spotCutoff_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].spotCutoff);
        }
        private void spotCosCutoff_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].spotCosCutoff);
        }

        private void spotDirection_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].spotDirection);
        }
        private void spotExponent_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].spotExponent);
        }

        private void spotCutoff_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].spotCutoff);
        }
        private void spotCosCutoff_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].spotCosCutoff);
        }

        private void spotDirection_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].spotDirection);
        }
        private void spotExponent_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].spotExponent);
        }

        private void spotCutoff_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].spotCutoff);
        }
        private void spotCosCutoff_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].spotCosCutoff);
        }
        private void constantAttenuation_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].ConstantAttenuation);
        }
        private void constantAttenuation_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].ConstantAttenuation);
        }
        private void constantAttenuation_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].ConstantAttenuation);
        }
        private void constantAttenuation_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].ConstantAttenuation);
        }
        private void constantAttenuation_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].ConstantAttenuation);
        }
        private void constantAttenuation_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].ConstantAttenuation);
        }
        private void constantAttenuation_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].ConstantAttenuation);
        }
        private void constantAttenuation_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].ConstantAttenuation);
        }
        private void constantAttenuation_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].ConstantAttenuation);
        }
        private void constantAttenuation_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].ConstantAttenuation);
        }
        private void constantAttenuation_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].ConstantAttenuation);
        }
        private void constantAttenuation_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].ConstantAttenuation);
        }
        private void constantAttenuation_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].ConstantAttenuation);
        }
        private void constantAttenuation_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].ConstantAttenuation);
        }
        private void constantAttenuation_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].ConstantAttenuation);
        }
        private void constantAttenuation_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].ConstantAttenuation);
        }
        private void ambient_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Ambient);
        }
        private void linearAttenuation_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].LinearAttenuation);
        }
        private void quadraticAttenuationt_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].QuadraticAttenuation);
        }
        private void diffuse_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Diffuse);
        }
        private void Specular_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[0].Specular);
        }
        private void position_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[0].Position));
        }
        private void enabled_0_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;

            sender.SetValue(Device.Lights[0].Enable);
        }


        private void ambient_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].Ambient);
        }
        private void linearAttenuation_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].LinearAttenuation);
        }
        private void quadraticAttenuationt_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].QuadraticAttenuation);
        }
        private void diffuse_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].Diffuse);
        }
        private void Specular_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].Specular);
        }
        private void position_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[1].Position));
        }
        private void enabled_1_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[1].Enable);
        }



        private void ambient_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].Ambient);
        }
        private void linearAttenuation_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].LinearAttenuation);
        }
        private void quadraticAttenuationt_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].QuadraticAttenuation);
        }
        private void diffuse_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].Diffuse);
        }
        private void Specular_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].Specular);
        }
        private void position_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[2].Position));
        }
        private void enabled_2_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[2].Enable);
        }



        private void ambient_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].Ambient);
        }
        private void linearAttenuation_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].LinearAttenuation);
        }
        private void quadraticAttenuationt_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].QuadraticAttenuation);
        }
        private void diffuse_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].Diffuse);
        }
        private void Specular_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].Specular);
        }
        private void position_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[3].Position));
        }
        private void enabled_3_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[3].Enable);
        }



        private void ambient_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].Ambient);
        }
        private void linearAttenuation_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].LinearAttenuation);
        }
        private void quadraticAttenuationt_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].QuadraticAttenuation);
        }
        private void diffuse_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].Diffuse);
        }
        private void Specular_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].Specular);
        }
        private void position_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[4].Position));
        }
        private void enabled_4_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[4].Enable);
        }



        private void ambient_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].Ambient);
        }
        private void linearAttenuation_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].LinearAttenuation);
        }
        private void quadraticAttenuationt_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].QuadraticAttenuation);
        }
        private void diffuse_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].Diffuse);
        }
        private void Specular_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].Specular);
        }
        private void position_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[5].Position));
        }
        private void enabled_5_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[5].Enable);
        }


        private void ambient_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].Ambient);
        }
        private void linearAttenuation_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].LinearAttenuation);
        }
        private void quadraticAttenuationt_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].QuadraticAttenuation);
        }
        private void diffuse_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].Diffuse);
        }
        private void Specular_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].Specular);
        }
        private void position_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[6].Position));
        }
        private void enabled_6_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[6].Enable);
        }


        private void ambient_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].Ambient);
        }
        private void linearAttenuation_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].LinearAttenuation);
        }
        private void quadraticAttenuationt_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].QuadraticAttenuation);
        }
        private void diffuse_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].Diffuse);
        }
        private void Specular_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].Specular);
        }
        private void position_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[7].Position));
        }
        private void enabled_7_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[7].Enable);
        }


        private void ambient_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].Ambient);
        }
        private void linearAttenuation_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].LinearAttenuation);
        }
        private void quadraticAttenuationt_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].QuadraticAttenuation);
        }
        private void diffuse_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].Diffuse);
        }
        private void Specular_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].Specular);
        }
        private void position_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[8].Position));
        }
        private void enabled_8_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[8].Enable);
        }


        private void ambient_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].Ambient);
        }
        private void linearAttenuation_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].LinearAttenuation);
        }
        private void quadraticAttenuationt_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].QuadraticAttenuation);
        }
        private void diffuse_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].Diffuse);
        }
        private void Specular_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].Specular);
        }
        private void position_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[9].Position));
        }
        private void enabled_9_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[9].Enable);
        }


        private void ambient_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].Ambient);
        }
        private void linearAttenuation_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].LinearAttenuation);
        }
        private void quadraticAttenuationt_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].QuadraticAttenuation);
        }
        private void diffuse_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].Diffuse);
        }
        private void Specular_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].Specular);
        }
        private void position_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[10].Position));
        }
        private void enabled_10_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[10].Enable);
        }


        private void ambient_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].Ambient);
        }
        private void linearAttenuation_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].LinearAttenuation);
        }
        private void quadraticAttenuationt_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].QuadraticAttenuation);
        }
        private void diffuse_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].Diffuse);
        }
        private void Specular_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].Specular);
        }
        private void position_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[11].Position));
        }
        private void enabled_11_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[11].Enable);
        }



        private void ambient_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].Ambient);
        }
        private void linearAttenuation_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].LinearAttenuation);
        }
        private void quadraticAttenuationt_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].QuadraticAttenuation);
        }
        private void diffuse_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].Diffuse);
        }
        private void Specular_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].Specular);
        }
        private void position_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[12].Position));
        }
        private void enabled_12_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[12].Enable);
        }



        private void ambient_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].Ambient);
        }
        private void linearAttenuation_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].LinearAttenuation);
        }
        private void quadraticAttenuationt_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].QuadraticAttenuation);
        }
        private void diffuse_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].Diffuse);
        }
        private void Specular_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].Specular);
        }
        private void position_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[13].Position));
        }
        private void enabled_13_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[13].Enable);
        }



        private void ambient_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].Ambient);
        }
        private void linearAttenuation_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].LinearAttenuation);
        }
        private void quadraticAttenuationt_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].QuadraticAttenuation);
        }
        private void diffuse_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].Diffuse);
        }
        private void Specular_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].Specular);
        }
        private void position_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[14].Position));
        }
        private void enabled_14_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].Enable);
        }
        private void ambient_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[14].Ambient);
        }
        private void linearAttenuation_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].LinearAttenuation);
        }
        private void quadraticAttenuationt_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].QuadraticAttenuation);
        }
        private void diffuse_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].Diffuse);
        }
        private void Specular_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].Specular);
        }
        private void position_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(toFloat(Device.Lights[15].Position));
        }
        private void enabled_15_Update(Field sender, object UpdateObject)
        {
            OpenGlDevice Device = UpdateObject as OpenGlDevice;
            sender.SetValue(Device.Lights[15].Enable);
        }

    }
}

