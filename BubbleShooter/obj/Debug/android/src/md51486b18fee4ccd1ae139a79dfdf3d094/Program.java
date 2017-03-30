package md51486b18fee4ccd1ae139a79dfdf3d094;


public class Program
	extends md5cedead65730cfb9c4b33fbfd5914d87f.AndroidGameActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("BubbleShooter.Program, BubbleShooter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Program.class, __md_methods);
	}


	public Program () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Program.class)
			mono.android.TypeManager.Activate ("BubbleShooter.Program, BubbleShooter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
