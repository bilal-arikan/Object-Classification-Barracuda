import coremltools as ct

print("step 1")
# Convert a Caffe model to a classifier in Core ML
model = ct.converters.caffe.convert(
    ('./caffemodels/oxford102_2.caffemodel', './caffemodels/oxford102.prototxt'),
    image_input_names="data",
    predicted_feature_name='./oxford102_labels.txt'
)
print("step 2")

# Now save the model
model.save('oxford102_2.mlmodel')
print("Completed")
