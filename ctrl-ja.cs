using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CtrlJa {
  public static class Program {
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private const int WM_IME_CONTROL = 0x0283;
    private const int IMC_SETOPENSTATUS = 0x0006;

    private static HookProc hookProc = HookCallback;
    private static IntPtr hookId = IntPtr.Zero;

    private static Keys keydownKey;

    public static void Main() {
      Console.CancelKeyPress += new ConsoleCancelEventHandler(Exit);

      hookId = SetHook(hookProc);
      Application.Run();
      UnhookWindowsHookEx(hookId);
    }

    private static void Exit (object sender, ConsoleCancelEventArgs args) {
      Application.Exit();
    }

    private static IntPtr SetHook(HookProc hookProc) {
      IntPtr moduleHandle = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
      return SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, moduleHandle, 0);
    }

    private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static void SwitchIme(int status) {
      IntPtr hFgWnd = GetForegroundWindow();
      SendMessageA(ImmGetDefaultIMEWnd(hFgWnd), WM_IME_CONTROL, IMC_SETOPENSTATUS, status);
    }

    private static void HandleKeyMessage(int nCode, IntPtr wParam, IntPtr lParam) {
      if (nCode < 0) {
        return;
      }
      if (wParam == (IntPtr)WM_KEYDOWN) {
        int vkCode = Marshal.ReadInt32(lParam);
        keydownKey = (Keys)vkCode;
      } else if (wParam == (IntPtr)WM_KEYUP) {
        int vkCode = Marshal.ReadInt32(lParam);
        Keys keyupKey = (Keys)vkCode;
        if (keydownKey != keyupKey) {
          return;
        }
        if (keyupKey == Keys.LControlKey) {
          SwitchIme(0);
        } else if (keyupKey == Keys.RControlKey) {
          SwitchIme(1);
        }
      }
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
      HandleKeyMessage(nCode, wParam, lParam);
      return CallNextHookEx(hookId, nCode, wParam, lParam);
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessageA(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("imm32.dll")]
    private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
  }
}