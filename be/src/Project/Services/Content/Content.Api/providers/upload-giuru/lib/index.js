module.exports = {
    init(providerOptions) {
      // init your provider if necessary
  
      return {
        upload(file) {
          // upload the file in the provider
          // file content is accessible by `file.buffer`
        },
        uploadStream(file) {
          // upload the file in the provider
          // file content is accessible by `file.stream`
        },
        delete(file) {
          // delete the file in the provider
        },
      };
    },
};