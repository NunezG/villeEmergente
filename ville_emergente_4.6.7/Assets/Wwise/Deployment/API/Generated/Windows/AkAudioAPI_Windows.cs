#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN
/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 2.0.11
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */


public enum AkAudioAPI {
  AkAPI_XAudio2 = 1 << 0,
  AkAPI_DirectSound = 1 << 1,
  AkAPI_Default = AkAPI_XAudio2|AkAPI_DirectSound,
  AkAPI_Dummy = 1 << 2
}
#endif // #if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN