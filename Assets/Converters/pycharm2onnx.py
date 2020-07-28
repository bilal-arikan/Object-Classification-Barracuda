# network
net = ...

# Input to the model
x = torch.randn(1, 3, 256, 256)

# Export the model
torch.onnx.export(net,                       # model being run
                  x,                         # model input (or a tuple for multiple inputs)
                  "example.onnx",            # where to save the model (can be a file or file-like object)
                  export_params=True,        # store the trained parameter weights inside the model file
                  opset_version=9,           # the ONNX version to export the model to
                  do_constant_folding=True,  # whether to execute constant folding for optimization
                  input_names = ['X'],       # the model's input names
                  output_names = ['Y']       # the model's output names
                  )