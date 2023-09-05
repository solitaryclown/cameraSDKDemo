using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using log4net;
using MvCamCtrl.NET;
using static MvCamCtrl.NET.MyCamera;
namespace HK_SDK_DEMO_回调采图
{
    public partial class GrabImage_Callback : Form
    {
        private ILog logger;
        private MV_CC_DEVICE_INFO_LIST camList = new MV_CC_DEVICE_INFO_LIST();
        private MyCamera myCamera=new MyCamera();
        cbOutputExdelegate ImageCallBack;
        string version;
        HImage hImage;
        HWindow hWindow;
        public GrabImage_Callback()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(this.GetType());

            uint iVersion = MyCamera.MV_CC_GetSDKVersion_NET();
            uint n1 = (iVersion & 0xFF000000) >> 24;
            uint n2 = (iVersion & 0x00FF0000) >> 16;
            uint n3 = (iVersion & 0x0000FF00) >> 8;
            uint n4 = (iVersion & 0x000000FF);
            version = n1 + "." + n2 + "." + n3 + "." + n4;
            logger.Info("SDK版本号：" + version);
            this.lbVersion.Text = version;

            InitDisplayWindow();
        }

        private void InitDisplayWindow()
        {
            hWindow = new HWindow();
            int hWindowWidth = pictureBox1.Width;
            int hWindowHeight =pictureBox1.Height;
            HTuple windowId = pictureBox1.Handle;
            hWindow.OpenWindow(0, 0, hWindowWidth, hWindowHeight, windowId, "visible", "");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.cbCamList.Items.Clear();

            uint type = MV_GIGE_DEVICE | MV_USB_DEVICE | MV_UNKNOW_DEVICE;
            int ret = MV_CC_EnumDevices_NET(type, ref camList);
            if(ret!=MV_OK)
            {
                Error("MV_CC_EnumDevices_NET出错",ret);
            }
            //遍历，添加到cbCamList
            for (int i = 0; i < camList.nDeviceNum; i++)
            {
                var device = (MV_CC_DEVICE_INFO)Marshal.PtrToStructure(camList.pDeviceInfo[i], typeof(MV_CC_DEVICE_INFO));
                uint layerType = device.nTLayerType;
                string id = string.Empty;
                switch (layerType)
                {
                    case MV_GIGE_DEVICE:
                        MV_GIGE_DEVICE_INFO gigeInfo = (MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MV_GIGE_DEVICE_INFO));
                        id = "GEV" + ":" + gigeInfo.chUserDefinedName + ":" + gigeInfo.chSerialNumber;
                        break;
                    case MV_USB_DEVICE:
                        MV_USB3_DEVICE_INFO usbInfo = (MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MV_USB3_DEVICE_INFO));
                        id = "GEV" + ":" + usbInfo.chUserDefinedName + ":" + usbInfo.chSerialNumber;
                        break;
                    default:
                        break;
                }
                this.cbCamList.Items.Add(id);
                
            }

            if(camList.nDeviceNum>0)
            {
                this.cbCamList.SelectedIndex = 0;
            }
        }

        private void Error(string msg, int ret)
        {

            string errCode = "0x"+ret.ToString("X8");
            logger.Error(msg + "：" + errCode);
        }

        private void btnOpenDevice_Click(object sender, EventArgs e)
        {
            int index = this.cbCamList.SelectedIndex;
            if(index<0)
            {
                Error("无选中设备！", -1);
                return;
            }
            var deviceInfo=(MV_CC_DEVICE_INFO)Marshal.PtrToStructure(camList.pDeviceInfo[index], typeof(MV_CC_DEVICE_INFO));
            int ret = myCamera.MV_CC_CreateDevice_NET(ref deviceInfo);
            if(ret!=MV_OK)
            {
                Error("MV_CC_CreateDevice_NET 错误", ret);
                return;
            }
            ret = myCamera.MV_CC_OpenDevice_NET();
            if (ret != MV_OK)
            {
                Error("MV_CC_OpenDevice_NET 错误", ret);
                return;
            }
            Init();
            myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            myCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            GetParams();
            //注册回调函数
            ImageCallBack = new cbOutputExdelegate(ImageCallBackFunc);
            IntPtr pUser = IntPtr.Zero;
            myCamera.MV_CC_RegisterImageCallBackEx_NET(ImageCallBack,pUser);
            
        }

        private void ImageCallBackFunc(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            logger.Info("采集到一帧图像，回调处理....");
            HObject himgObj;
            //转成halcon对象
            HOperatorSet.GenImage1Extern(out himgObj, "byte", pFrameInfo.nHeight, pFrameInfo.nWidth
                , pData,IntPtr.Zero);
            //保存图像
            string time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            string filePath = "E:\\temp\\" + time + ".bmp";
            new HImage(himgObj).WriteImage("bmp",0,filePath);
            HalconDisplay(himgObj,pFrameInfo.nHeight, pFrameInfo.nWidth);
        }

        private void HalconDisplay(HObject himgObj,int height,int width)
        {
            hWindow.SetPart(0, 0, height - 1, width - 1);
            hWindow.DispObj(himgObj);
        }

        /// <summary>
        /// 获取当前设备参数
        /// </summary>
        private void GetParams()
        {
            MVCC_FLOATVALUE temp = new MVCC_FLOATVALUE();
            myCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref temp);
            this.tbxExposureTime.Text = temp.fCurValue.ToString("F2");
            myCamera.MV_CC_GetFloatValue_NET("Gain", ref temp);
            this.tbxGain.Text = temp.fCurValue.ToString("F2");
            myCamera.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref temp);
            this.tbxFrameRate.Text = temp.fCurValue.ToString("F2");
        }

        private void Init()
        {
            MV_CC_DEVICE_INFO device=new MV_CC_DEVICE_INFO();
            myCamera.MV_CC_GetDeviceInfo_NET(ref device);
            // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
            {
                int nPacketSize = myCamera.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    int nRet = myCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != MyCamera.MV_OK)
                    {
                        Error("Set Packet Size failed!", nRet);
                    }
                }
                else
                {
                    Error("Get Packet Size failed!", nPacketSize);
                }
            }
        }

        private void btnCloseDevice_Click(object sender, EventArgs e)
        {
            int ret = myCamera.MV_CC_CloseDevice_NET();
            if(ret!=MV_OK)
            {
                Error("MV_CC_CloseDevice_NET  错误", ret);
                return;
            }
            ret = myCamera.MV_CC_DestroyDevice_NET();
            if (ret != MV_OK)
            {
                Error("MV_CC_DestroyDevice_NET  错误", ret);
                return;
            }
        }

        private void rbTriggerOn_CheckedChanged(object sender, EventArgs e)
        {
            if(rbTriggerOn.Checked)
            {
                int ret = myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                Error("触发模式更改为ON错误", ret);
            }
        }

        private void rbTriggerOff_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTriggerOff.Checked)
            {
                int ret = myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                Error("触发模式更改为OFF错误", ret);
            }
        }

        private void rbSoftware_CheckedChanged(object sender, EventArgs e)
        {
            SetTriggerSource();
        }

        private void SetTriggerSource()
        {
            if(rbSoftware.Checked)
            {
                int ret = myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                if(ret!=MV_OK)
                {
                    Error("设置软触发错误", ret);
                }
            }
            else
            {
                int ret = myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
                Error("设置硬触发line0错误", ret);
            }
        }

        private void rbHardware_CheckedChanged(object sender, EventArgs e)
        {
            SetTriggerSource();
        }

        private void btnCommandSoftware_Click(object sender, EventArgs e)
        {
            int ret = myCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if(ret!=MV_OK)
            {
                Error("软触发一次失败!",ret);
            }
        }

        private void tbxExposureTime_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnStartGrabbing_Click(object sender, EventArgs e)
        {
            int ret = myCamera.MV_CC_StartGrabbing_NET();
            if(ret!=MV_OK)
            {
                Error("MV_CC_StartGrabbing_NET失败!", ret);
                return;
            }
        }

        private void btnStopGrabbing_Click(object sender, EventArgs e)
        {
            int ret = myCamera.MV_CC_StopGrabbing_NET();
            if (ret != MV_OK)
            {
                Error("MV_CC_StartGrabbing_NET失败!", ret);
                return;
            }
        }
    }
}
