import coremltools
import onnxmltools

print("step 1")
coreml_model = coremltools.utils.load_spec(
    './mlmodels/oxford102_2.mlmodel')
print("step 2")
onnx_model = onnxmltools.convert_coreml(coreml_model, target_opset=9)
print("step 3")
onnxmltools.utils.save_model(onnx_model, './onnxmodels/oxford102_2.onnx')
print("completed")
