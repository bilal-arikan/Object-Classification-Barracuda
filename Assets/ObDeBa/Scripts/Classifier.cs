using System.Reflection.Emit;
using System;
using Unity.Barracuda;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class Classifier : MonoBehaviour
{
    public NNModel modelFile;
    public TextAsset labelsFile;

    public int IMAGE_SIZE = 224;
    // private const int IMAGE_MEAN = 0;
    private const int IMAGE_MEAN = 127;
    // private const float IMAGE_STD = 255f;
    private const float IMAGE_STD = 127.5f;
    public string INPUT_NAME = "input";
    public string OUTPUT_NAME = "softmax_Y";
    // private const string OUTPUT_NAME = "MobilenetV2/Predictions/Reshape_1";

    private IWorker worker;
    private string[] labels;


    public void Start()
    {
        this.labels = Regex.Split(this.labelsFile.text, "\n|\r|\r\n")
            .Where(s => !String.IsNullOrEmpty(s)).ToArray();
        var model = ModelLoader.Load(this.modelFile, false);
        this.worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }


    private int i = 0;
    public IEnumerator Classify(Color32[] picture, System.Action<List<KeyValuePair<string, float>>> callback)
    // public IEnumerator Classify(Texture picture, System.Action<List<KeyValuePair<string, float>>> callback)
    {
        if (worker == null)
        {
            Debug.LogError("Classify Worker null");
            yield break;
        }
        var map = new List<KeyValuePair<string, float>>();

        using (var tensor = TransformInput(picture, IMAGE_SIZE, IMAGE_SIZE))
        // using (var tensor = TransformInput(picture))
        {
            var inputs = new Dictionary<string, Tensor>();
            inputs.Add(INPUT_NAME, tensor);
            var enumerator = this.worker.ExecuteAsync(inputs);

            while (enumerator.MoveNext())
            {
                i++;
                if (i >= 20)
                {
                    i = 0;
                    yield return null;
                }
            };

            // this.worker.Execute(inputs);
            // Execute() scheduled async job on GPU, waiting till completion
            // yield return new WaitForSeconds(0.5f);

            var output = worker.PeekOutput(OUTPUT_NAME);
            Debug.Log(output.shape.ToString());
            // Debug.Log(string.Join("\n", output.ToReadOnlyArray()));
            // string res = "";
            for (int i = 0; i < labels.Length; i++)
            {
                // res += output[0, 0, 0, i] + "-" + "\n";
                map.Add(new KeyValuePair<string, float>(labels[i].ToString(), output[i] * 100));
            }
            // Debug.Log(res);
        }

        callback(map.OrderByDescending(x => x.Value).ToList());
    }

    public static Tensor TransformInput(Texture texture)
    {
        return new Tensor(texture, 3);
    }
    public static Tensor TransformInput(Color32[] pic, int width, int height)
    {
        // var IMAGE_MEAN = 0f;
        // var IMAGE_STD = 1f;
        float[] floatValues = new float[width * height * 3];

        for (int i = 0; i < pic.Length; ++i)
        {
            var color = pic[i];
            floatValues[i * 3 + 0] = (color.r - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 1] = (color.g - IMAGE_MEAN) / IMAGE_STD;
            floatValues[i * 3 + 2] = (color.b - IMAGE_MEAN) / IMAGE_STD;
        }
        // for (int i = 0; i < pic.Length; ++i)
        // {
        //     var color = pic[i];
        //     floatValues[i + (0 * pic.Length)] = (color.r - IMAGE_MEAN) / IMAGE_STD;
        //     floatValues[i + (1 * pic.Length)] = (color.g - IMAGE_MEAN) / IMAGE_STD;
        //     floatValues[i + (2 * pic.Length)] = (color.b - IMAGE_MEAN) / IMAGE_STD;
        // }
        return new Tensor(1, height, width, 3, floatValues);
    }
}