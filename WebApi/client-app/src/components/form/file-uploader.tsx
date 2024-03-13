import React, { useState } from "react";
import { Button } from "react-bootstrap";
import Dropzone from "react-dropzone";
import classNames from "classnames";

export interface FileType {
  FullPath?: string;
  Name?: string;
  formattedSize?: string;
}

export interface FileUploaderProps {
  onFilesSelected: (files: File[]) => void;
  onFilesRemoved: (index: number) => void;
  showPreview: boolean | false;
  selectedFiles: FileType[];
}

const FileUploader = (props: FileUploaderProps) => {
  // const [selectedFiles, setSelectedFiles] = useState<FileType[]>([]);

  /**
   * Handled the accepted files and shows the preview
   */
  const handleAcceptedFiles = (files: File[]) => {
    // var allFiles = files;

    // if (props.showPreview) {
    //   (files || []).map((file) =>
    //     Object.assign(file, {
    //       preview:
    //         file["type"].split("/")[0] === "image"
    //           ? URL.createObjectURL(file)
    //           : null,
    //       formattedSize: formatBytes(file.size),
    //     })
    //   );
    //   allFiles = [...selectedFiles];
    //   allFiles.push(...files);
    //   setSelectedFiles(allFiles);
    // }

    if (props.onFilesSelected) props.onFilesSelected(files);
  };

  /**
   * Formats the size
   */
  const formatBytes = (bytes: number, decimals: number = 2) => {
    if (bytes === 0) return "0 Bytes";
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];

    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + " " + sizes[i];
  };

  /*
   * Removes the selected file
   */
  const removeFile = (e: any, fileIndex: number) => {
    props.onFilesRemoved(fileIndex);
    // const newFiles = [...props.selectedFiles];
    // newFiles.splice(fileIndex, 1);
    // setSelectedFiles(newFiles);
    // if (props.onFilesSelected) props.onFilesSelected(newFiles);
  };

  return (
    <>
      <Dropzone
        {...props}
        onDrop={(acceptedFiles) => handleAcceptedFiles(acceptedFiles)}
      >
        {({ getRootProps, getInputProps }) => (
          <div
            className={classNames("dropzone", "dz-clickable", {
              "dz-started":
                props.selectedFiles && props.selectedFiles.length > 0,
            })}
          >
            <div {...getRootProps()}>
              <div className="dz-message needsclick">
                <input {...getInputProps()} />
                <i className="h1 text-muted uil-cloud-upload"></i>
                <h3>Drop files here or click to upload.</h3>
              </div>
              {props.showPreview &&
                (props.selectedFiles || []).map((f, i) => {
                  return (
                    <React.Fragment key={i}>
                      {f.FullPath && (
                        <div
                          onClick={(e) => e.stopPropagation()}
                          className="dz-preview dz-processing dz-error dz-complete dz-image-preview"
                        >
                          <div className="dz-image">
                            <img
                              key={i}
                              data-dz-thumbnail=""
                              alt={f.Name}
                              src={f.FullPath}
                            />
                          </div>

                          <div className="dz-details">
                            {/* <div className="dz-size">
                              <span data-dz-size="">
                                <strong>{f.formattedSize}</strong> KB
                              </span>
                            </div> */}
                            <div className="dz-filename">
                              <span data-dz-name="">{f.Name}</span>
                            </div>
                          </div>

                          <div className="dz-remove">
                            <Button
                              variant=""
                              onClick={(e) => removeFile(e, i)}
                            >
                              <i className="uil uil-multiply"></i>
                            </Button>
                          </div>
                        </div>
                      )}
                      {!f.FullPath && (
                        <div
                          onClick={(e) => e.stopPropagation()}
                          className="dz-preview dz-file-preview dz-processing dz-error dz-complete"
                        >
                          <div className="dz-image">
                            <img data-dz-thumbnail="" alt="" />
                          </div>
                          <div className="dz-details">
                            <div className="dz-size">
                              <span data-dz-size="">
                                <strong>{f.formattedSize}</strong> KB
                              </span>
                            </div>
                            <div className="dz-filename">
                              <span data-dz-name="">{f.Name}</span>
                            </div>
                          </div>

                          <div className="dz-remove">
                            <Button
                              variant=""
                              onClick={(e) => removeFile(e, i)}
                            >
                              <i className="uil uil-multiply"></i>
                            </Button>
                          </div>
                        </div>
                      )}
                    </React.Fragment>
                  );
                })}
            </div>
          </div>
        )}
      </Dropzone>
    </>
  );
};

FileUploader.defaultProps = {
  showPreview: true,
};

export default FileUploader;
