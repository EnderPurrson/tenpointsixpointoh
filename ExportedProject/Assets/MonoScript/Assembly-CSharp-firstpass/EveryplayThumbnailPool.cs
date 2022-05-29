using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EveryplayThumbnailPool : MonoBehaviour
{
	public int thumbnailCount = 4;

	public int thumbnailWidth = 128;

	public bool pixelPerfect;

	public bool takeRandomShots = true;

	public TextureFormat textureFormat = TextureFormat.RGBA32;

	public bool dontDestroyOnLoad = true;

	public bool allowOneInstanceOnly = true;

	private bool npotSupported;

	private bool initialized;

	private int currentThumbnailTextureIndex;

	private float nextRandomShotTime;

	private int thumbnailHeight;

	public float aspectRatio
	{
		get;
		private set;
	}

	public int availableThumbnailCount
	{
		get;
		private set;
	}

	public Vector2 thumbnailScale
	{
		get;
		private set;
	}

	public Texture2D[] thumbnailTextures
	{
		get;
		private set;
	}

	public EveryplayThumbnailPool()
	{
	}

	private void Awake()
	{
		if (!this.allowOneInstanceOnly || (int)UnityEngine.Object.FindObjectsOfType(base.GetType()).Length <= 1)
		{
			if (this.dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			Everyplay.ReadyForRecording += new Everyplay.ReadyForRecordingDelegate(this.OnReadyForRecording);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Initialize()
	{
		if (!this.initialized && Everyplay.IsRecordingSupported())
		{
			this.thumbnailWidth = Mathf.Clamp(this.thumbnailWidth, 32, 2048);
			this.aspectRatio = (float)Mathf.Min(Screen.width, Screen.height) / (float)Mathf.Max(Screen.width, Screen.height);
			this.thumbnailHeight = (int)((float)this.thumbnailWidth * this.aspectRatio);
			this.npotSupported = false;
			this.npotSupported = SystemInfo.npotSupport != NPOTSupport.None;
			int num = Mathf.NextPowerOfTwo(this.thumbnailWidth);
			int num1 = Mathf.NextPowerOfTwo(this.thumbnailHeight);
			this.thumbnailTextures = new Texture2D[this.thumbnailCount];
			for (int i = 0; i < this.thumbnailCount; i++)
			{
				this.thumbnailTextures[i] = new Texture2D((!this.npotSupported ? num : this.thumbnailWidth), (!this.npotSupported ? num1 : this.thumbnailHeight), this.textureFormat, false);
				this.thumbnailTextures[i].wrapMode = TextureWrapMode.Clamp;
			}
			this.currentThumbnailTextureIndex = 0;
			Everyplay.SetThumbnailTargetTexture(this.thumbnailTextures[this.currentThumbnailTextureIndex]);
			this.SetThumbnailTargetSize();
			Everyplay.ThumbnailTextureReady += new Everyplay.ThumbnailTextureReadyDelegate(this.OnThumbnailReady);
			Everyplay.RecordingStarted += new Everyplay.RecordingStartedDelegate(this.OnRecordingStarted);
			this.initialized = true;
		}
	}

	private void OnDestroy()
	{
		Everyplay.ReadyForRecording -= new Everyplay.ReadyForRecordingDelegate(this.OnReadyForRecording);
		if (this.initialized)
		{
			Everyplay.SetThumbnailTargetTexture(null);
			Everyplay.RecordingStarted -= new Everyplay.RecordingStartedDelegate(this.OnRecordingStarted);
			Everyplay.ThumbnailTextureReady -= new Everyplay.ThumbnailTextureReadyDelegate(this.OnThumbnailReady);
			Texture2D[] texture2DArray = this.thumbnailTextures;
			for (int i = 0; i < (int)texture2DArray.Length; i++)
			{
				Texture2D texture2D = texture2DArray[i];
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
				}
			}
			this.thumbnailTextures = null;
			this.initialized = false;
		}
	}

	private void OnReadyForRecording(bool ready)
	{
		if (ready)
		{
			this.Initialize();
		}
	}

	private void OnRecordingStarted()
	{
		this.availableThumbnailCount = 0;
		this.currentThumbnailTextureIndex = 0;
		Everyplay.SetThumbnailTargetTexture(this.thumbnailTextures[this.currentThumbnailTextureIndex]);
		this.SetThumbnailTargetSize();
		if (this.takeRandomShots)
		{
			Everyplay.TakeThumbnail();
			this.nextRandomShotTime = Time.time + UnityEngine.Random.Range(3f, 15f);
		}
	}

	private void OnThumbnailReady(Texture2D texture, bool portrait)
	{
		if (this.thumbnailTextures[this.currentThumbnailTextureIndex] == texture)
		{
			this.currentThumbnailTextureIndex++;
			if (this.currentThumbnailTextureIndex >= (int)this.thumbnailTextures.Length)
			{
				this.currentThumbnailTextureIndex = 0;
			}
			if (this.availableThumbnailCount < (int)this.thumbnailTextures.Length)
			{
				EveryplayThumbnailPool everyplayThumbnailPool = this;
				everyplayThumbnailPool.availableThumbnailCount = everyplayThumbnailPool.availableThumbnailCount + 1;
			}
			Everyplay.SetThumbnailTargetTexture(this.thumbnailTextures[this.currentThumbnailTextureIndex]);
			this.SetThumbnailTargetSize();
		}
	}

	private void SetThumbnailTargetSize()
	{
		int num = Mathf.NextPowerOfTwo(this.thumbnailWidth);
		int num1 = Mathf.NextPowerOfTwo(this.thumbnailHeight);
		if (this.npotSupported)
		{
			Everyplay.SetThumbnailTargetTextureWidth(this.thumbnailWidth);
			Everyplay.SetThumbnailTargetTextureHeight(this.thumbnailHeight);
			this.thumbnailScale = new Vector2(1f, 1f);
		}
		else if (!this.pixelPerfect)
		{
			Everyplay.SetThumbnailTargetTextureWidth(num);
			Everyplay.SetThumbnailTargetTextureHeight(num1);
			this.thumbnailScale = new Vector2(1f, 1f);
		}
		else
		{
			Everyplay.SetThumbnailTargetTextureWidth(this.thumbnailWidth);
			Everyplay.SetThumbnailTargetTextureHeight(this.thumbnailHeight);
			this.thumbnailScale = new Vector2((float)this.thumbnailWidth / (float)num, (float)this.thumbnailHeight / (float)num1);
		}
	}

	private void Start()
	{
		if (base.enabled)
		{
			this.Initialize();
		}
	}

	private void Update()
	{
		if (this.takeRandomShots && Everyplay.IsRecording() && !Everyplay.IsPaused() && Time.time > this.nextRandomShotTime)
		{
			Everyplay.TakeThumbnail();
			this.nextRandomShotTime = Time.time + UnityEngine.Random.Range(3f, 15f);
		}
	}
}