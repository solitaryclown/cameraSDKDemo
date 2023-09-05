using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MvCamCtrl.NET;
using static MvCamCtrl.NET.MyCamera;

namespace HK_SDK_DEMO
{
    public partial class GrabImage : Form
    {
        MV_CC_DEVICE_INFO_LIST camList = new MV_CC_DEVICE_INFO_LIST();
        MyCamera myCamera = new MyCamera();
        bool isGrabbing = false;
        Thread processThread = null;
        IntPtr bufForDriver;
        IntPtr bufForSaveImage;
        uint bufSizeForSaveImage = 0;
        uint bufSizeForDriver = 0;
        private object lockOfBufForDriver = new object();
        MV_FRAME_OUT_INFO_EX frameInfo = new MV_FRAME_OUT_INFO_EX();

        private ILog logger;
        string version = string.Empty;
        public GrabImage()
        {

           
            InitializeComponent();
            logger = LogManager.GetLogger(this.GetType());
            uint iVersion = MyCamera.MV_CC_GetSDKVersion_NET();
            uint n1 = (iVersion & 0xFF000000) >> 24;
            uint n2 = (iVersion & 0x00FF0000) >> 16;
            uint n3 = (iVersion & 0x0000FF00) >> 8;
            uint n4 = (iVersion & 0x000000FF);
            version = n1 + "." + n2 + "."+ n3 + "." + n4;
            logger.Info("SDK版本号：" + version);
            this.lbVersion.Text = version;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void GrabImage_Load(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //清除
            this.cbCamList.Items.Clear();
            uint type =
                MyCamera.MV_GIGE_DEVICE |
                MyCamera.MV_USB_DEVICE |
                MyCamera.MV_UNKNOW_DEVICE;
            //获取设备列表
            MV_CC_EnumDevices_NET(type, ref camList);
            for (uint i = 0; i < camList.nDeviceNum; i++)
            {
                IntPtr ptr = camList.pDeviceInfo[i];
                MV_CC_DEVICE_INFO deviceInfo = (MV_CC_DEVICE_INFO)Marshal.PtrToStructure(ptr, typeof(MV_CC_DEVICE_INFO));
                string id = string.Empty;
                //GIGE设备
                if (deviceInfo.nTLayerType == MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MV_GIGE_DEVICE_INFO)ByteToStruct(deviceInfo.SpecialInfo.stGigEInfo, typeof(MV_GIGE_DEVICE_INFO));
                    id = "GEV:" + gigeInfo.chUserDefinedName + ":" + gigeInfo.chSerialNumber;
                }
                else
                {
                    var usbInfo = (MV_USB3_DEVICE_INFO)ByteToStruct(deviceInfo.SpecialInfo.stGigEInfo, typeof(MV_USB3_DEVICE_INFO));
                    id = "U3V:" + usbInfo.chUserDefinedName + ":" + usbInfo.chSerialNumber;
                }
                this.cbCamList.Items.Add(id);
            }
            if (camList.nDeviceNum > 0)
            {
                this.cbCamList.SelectedIndex = 0;
            }
        }

        private void cbCamList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 打开设备
        /// </summary>
        private void OpenDevice()
        {
            int index = cbCamList.SelectedIndex;
            if (camList.nDeviceNum == 0 || index < 0)
            {
                Error("无选中设备！");
                return;
            }
            IntPtr infoPtr = camList.pDeviceInfo[index];
            MV_CC_DEVICE_INFO deviceInfo = (MV_CC_DEVICE_INFO)Marshal.PtrToStructure(infoPtr, typeof(MV_CC_DEVICE_INFO));

            //
            int ret = myCamera.MV_CC_CreateDevice_NET(ref deviceInfo);
            if (ret != MV_OK)
            {
                Error("MV_CC_CreateDevice_NET错误");
                return;
            }
            ret = myCamera.MV_CC_OpenDevice_NET();
            if (ret != MV_OK)
            {
                myCamera.MV_CC_DestroyDevice_NET();
                Error("MV_CC_OpenDevice_NET错误");
                return;
            }
            SetCtrlWhenOpen();
            InitDevice(ref deviceInfo);
            GetParams();
        }

        private void GetParams()
        {
            MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = myCamera.MV_CC_GetFloatValue_NET("ExposureTime", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                tbxExposureTime.Text = stParam.fCurValue.ToString("F1");
            }

            nRet = myCamera.MV_CC_GetFloatValue_NET("Gain", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                tbxGain.Text = stParam.fCurValue.ToString("F1");
            }

            nRet = myCamera.MV_CC_GetFloatValue_NET("ResultingFrameRate", ref stParam);
            if (MyCamera.MV_OK == nRet)
            {
                tbxFrameRate.Text = stParam.fCurValue.ToString("F1");
            }
        }

        private void InitDevice(ref MV_CC_DEVICE_INFO deviceInfo)
        {
            if (deviceInfo.nTLayerType == MV_GIGE_DEVICE)
            {
                int packetSize = myCamera.MV_CC_GetOptimalPacketSize_NET();
                if (packetSize > 0)
                {
                    int ret = myCamera.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)packetSize);
                    if (ret != MV_OK)
                    {
                        Error("set packet size 错误");
                    }
                }
                else
                {
                    Error("MV_CC_GetOptimalPacketSize_NET 错误");
                }
            }
            //打开设备后默认设置成连续采集模式
            myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            myCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
        }

        private void Error(string msg)
        {
            MessageBox.Show(msg);
            logger.Error(msg);
        }

        private void btnOpenDevice_Click(object sender, EventArgs e)
        {
            OpenDevice();
        }

        private void SetCtrlWhenOpen()
        {
            Enabled(btnCloseDevice,btnStartGrabbing);
            Disabled(
                btnOpenDevice,btnStopGrabbing,btnCommandSoftware
                );
        }

        private void Disabled(params Control[] controls)
        {
            foreach (Control ctrl in controls)
            {
                ctrl.Enabled = false;
            }
        }

        private void Enabled(params Control[] controls)
        {
            foreach(Control ctrl in controls)
            {
                ctrl.Enabled = true;
            }
        }

        private void btnCloseDevice_Click(object sender, EventArgs e)
        {
            if (isGrabbing)
            {
                isGrabbing = false;
                processThread.Join();
            }
            if (bufForDriver != IntPtr.Zero)
            {
                Marshal.Release(bufForDriver);
            }
            if (bufForSaveImage != IntPtr.Zero)
            {
                Marshal.Release(bufForSaveImage);
            }

            myCamera.MV_CC_CloseDevice_NET();
            int ret = myCamera.MV_CC_DestroyDevice_NET();
            if (ret != MV_OK)
            {
                Error("关闭设备失败");
            }

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void rbTriggerOn_CheckedChanged(object sender, EventArgs e)
        {
            //开启触发模式
            if (rbTriggerOn.Checked)
            {
                myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
            }
        }

        private void rbTriggerOff_CheckedChanged(object sender, EventArgs e)
        {
            //关闭触发模式
            if (rbTriggerOn.Checked)
            {
                myCamera.MV_CC_SetEnumValue_NET("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
            }
        }

        private void btnGrabSingleImage_Click(object sender, EventArgs e)
        {
            isGrabbing = true;
            processThread = new Thread(processFunc);
            processThread.Start();

            //取流之前清除帧
            frameInfo.nFrameLen = 0;
            frameInfo.enPixelType = MvGvspPixelType.PixelType_Gvsp_Undefined;
            //开始采集
            int ret = myCamera.MV_CC_StartGrabbing_NET();
            if (ret != MV_OK)
            {
                isGrabbing = false;
                processThread.Join();
                Error("采图错误");
                return;
            }
        }
        private void btnStopGrabbing_Click(object sender, EventArgs e)
        {
            isGrabbing = false;
            processThread.Join();
            int ret = myCamera.MV_CC_StopGrabbing_NET();
            if (ret != MV_OK)
            {
                Error("停止采集错误");
            }
        }
        private void processFunc()
        {
            var param = new MVCC_INTVALUE();
            int ret = myCamera.MV_CC_GetIntValue_NET("PayloadSize", ref param);
            if (ret != MV_OK)
            {
                Error("get payload size failed");
                return;
            }
            uint payloadSize = param.nCurValue;
            if (payloadSize > bufSizeForDriver)
            {
                if (bufForDriver != IntPtr.Zero)
                {
                    Marshal.Release(bufForDriver);
                }
                bufSizeForDriver = payloadSize;
                //分配内存
                bufForDriver = Marshal.AllocHGlobal((int)bufSizeForDriver);

                if (bufForDriver == IntPtr.Zero)
                {
                    return;
                }
                MV_FRAME_OUT_INFO_EX stFrameInfo = new MV_FRAME_OUT_INFO_EX();
                MV_DISPLAY_FRAME_INFO stDisplayInfo = new MV_DISPLAY_FRAME_INFO();

                while (isGrabbing)
                {
                    lock (lockOfBufForDriver)
                    {
                        logger.Info("调用MV_CC_GetOneFrameTimeout_NET采图.");
                        ret = myCamera.MV_CC_GetOneFrameTimeout_NET(bufForDriver, payloadSize, ref stFrameInfo, 1000);
                        if (ret == MV_OK)
                        {
                            frameInfo = stFrameInfo;
                        }
                    }
                    
                    if (ret == MV_OK)
                    {
                        stDisplayInfo.hWnd = this.pictureBox1.Handle;
                        stDisplayInfo.pData = bufForDriver;
                        stDisplayInfo.nDataLen = stFrameInfo.nFrameLen;
                        stDisplayInfo.nHeight = stFrameInfo.nHeight;
                        stDisplayInfo.enPixelType = stFrameInfo.enPixelType;
                        myCamera.MV_CC_DisplayOneFrame_NET(ref stDisplayInfo);
                    }
                    else
                    {
                        if (rbTriggerOn.Checked)
                        {
                            Thread.Sleep(5);
                        }
                    }
                }
            }

        }

        private void rbSoftware_CheckedChanged(object sender, EventArgs e)
        {
            uint triggerSource;
            if(rbSoftware.Checked)
            {
                //软触发
                triggerSource = (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE;
            }
            else
            {
                triggerSource = (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0;
            }
            logger.Info("软触发一次.");
            myCamera.MV_CC_SetEnumValue_NET("TriggerSource", triggerSource);
        }

        private void btnCommandSoftware_Click(object sender, EventArgs e)
        {
            int ret = myCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (ret != MV_OK) 
            {
                Error("软触发执行一次失败！");
            }
        }

        private void btnSaveBMP_Click(object sender, EventArgs e)
        {
            if(!isGrabbing)
            {
                Error("未开始采图！");
                return;
            }
            IntPtr pTemp = IntPtr.Zero;
            var enDstPixelType = MvGvspPixelType.PixelType_Gvsp_Undefined;
            if(
                frameInfo.enPixelType==MvGvspPixelType.PixelType_Gvsp_Mono8
                ||frameInfo.enPixelType==MvGvspPixelType.PixelType_Gvsp_BGR8_Packed
                )
            {
                pTemp = bufForDriver;
                enDstPixelType = frameInfo.enPixelType;
            }
            else
            {
                UInt32 saveImageSize = 0;
                MV_PIXEL_CONVERT_PARAM stConverPixelParam = new MV_PIXEL_CONVERT_PARAM();

                lock(lockOfBufForDriver)
                {
                    if(frameInfo.nFrameLen==0)
                    {
                        Error("保存错误，frame len为0！");
                        return;
                    }

                    if(IsMonoData(frameInfo.enPixelType))
                    {
                        enDstPixelType = MvGvspPixelType.PixelType_Gvsp_Mono8;
                        saveImageSize = (uint)frameInfo.nWidth * frameInfo.nHeight;
                    }
                    else if(IsColorData(frameInfo.enPixelType))
                    {
                        enDstPixelType = MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;
                        saveImageSize = (uint)frameInfo.nWidth * frameInfo.nHeight * 3;
                    }
                    else
                    {
                        Error("错误，没有这种颜色格式：" + enDstPixelType.ToString());
                    }

                    if(bufSizeForSaveImage<saveImageSize)
                    {
                        if(bufForSaveImage!=IntPtr.Zero)
                        {
                            Marshal.Release(bufForSaveImage);
                        }
                        bufSizeForSaveImage = saveImageSize;
                        bufForSaveImage = Marshal.AllocHGlobal((int)bufSizeForSaveImage);
                    }
                    stConverPixelParam.nWidth = frameInfo.nWidth;
                    stConverPixelParam.nHeight = frameInfo.nHeight;
                    stConverPixelParam.pSrcData = bufForDriver;
                    stConverPixelParam.nSrcDataLen = frameInfo.nFrameLen;
                    stConverPixelParam.enSrcPixelType = frameInfo.enPixelType;
                    stConverPixelParam.enDstPixelType = enDstPixelType;
                    stConverPixelParam.pDstBuffer = bufForSaveImage;
                    stConverPixelParam.nDstBufferSize = bufSizeForSaveImage;
                    int ret = myCamera.MV_CC_ConvertPixelType_NET(ref stConverPixelParam);

                    if(ret!=MV_OK)
                    {
                        Error("转换像素类型错误！"+ret);
                        return;
                    }
                    pTemp = bufForSaveImage;
                }
            }

            lock(lockOfBufForDriver)
            {
                try
                {
                    if (enDstPixelType == MvGvspPixelType.PixelType_Gvsp_Mono8)
                    {
                        /*
                         * Mono8转bitmap
                         */
                        var bmp = new Bitmap(frameInfo.nWidth, frameInfo.nHeight, frameInfo.nWidth * 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, pTemp);
                        var palette = bmp.Palette;
                        //初始化调色板
                        for (int i = 0; i < 256; i++)
                        {
                            palette.Entries[i] = Color.FromArgb(i, i, i);
                        }
                        bmp.Palette = palette;
                        string time = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:fff");
                        bmp.Save(time + ".bmp", ImageFormat.Bmp);
                    }
                    else
                    {
                        /*
                         BGR8转bitmap
                         */
                        var bmp = new Bitmap(frameInfo.nWidth, frameInfo.nHeight, frameInfo.nWidth * 3, PixelFormat.Format24bppRgb, pTemp);
                        string time = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:fff");
                        bmp.Save(time + ".bmp", ImageFormat.Bmp);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
            }

        }
        /************************************************************************
        *  @fn     IsColorData()
        *  @brief  判断是否是彩色数据
        *  @param  enGvspPixelType         [IN]           像素格式
        *  @return 成功，返回0；错误，返回-1 
        ************************************************************************/
        private Boolean IsColorData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YCBCR411_8_CBYYCRYY:
                    return true;

                default:
                    return false;
            }
        }
        private Boolean IsMonoData(MyCamera.MvGvspPixelType enGvspPixelType)
        {
            switch (enGvspPixelType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;

                default:
                    return false;
            }
        }

        private void GrabImage_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void rbHardware_CheckedChanged(object sender, EventArgs e)
        {
            myCamera.MV_CC_SetEnumValue_NET("TriggerSource", (uint)MyCamera.MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
        }
    }
}