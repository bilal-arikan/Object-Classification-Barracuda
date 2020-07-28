import keras2onnx

# network
net = ...

# convert model to ONNX
onnx_model = keras2onnx.convert_keras(net,         # keras model
                                      name="example",           # the converted ONNX model internal name
                                      target_opset=9,           # the ONNX version to export the model to
                                      channel_first_inputs=None  # which inputs to transpose from NHWC to NCHW
                                      )

onnx.save_model(onnx_model, "example.onnx")
