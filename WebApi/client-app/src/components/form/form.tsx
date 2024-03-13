import { useTranslation } from "react-i18next";
import FeatherIcons from "feather-icons-react";
import { Button, Col, Form, Row } from "react-bootstrap";
import { CSSProperties, PropsWithChildren, useState } from "react";
import FormInput, { AppSelect, ToggleSwitch } from "./input";
import { OptionsOrGroups } from "react-select";
import { Option } from "react-bootstrap-typeahead";
import FileUploader, { FileType } from "./file-uploader";
import useFetch from "../../hooks/use-fetch";

export default function AppForm({
  onSubmit,
  defaultValues = {},
  validationSchema,
  files,
  ...props
}: PropsWithChildren<FormPropsType>) {
  const { postFile } = useFetch();
  const { t } = useTranslation();
  const [errors, setErrors] = useState<any>({});
  const [selectedFiles, setSelectedFiles] = useState<any>(files);
  async function handleFilesSelected(files: File[], fieldName: string) {
    const resp = (await postFile("/upload", files)) as any;
    setSelectedFiles({
      ...selectedFiles,
      [fieldName]: [...(selectedFiles[fieldName] || []), ...resp],
    });
  }
  function handleFileRemoved(fileIndex: number, fieldName: string) {
    const newFiles = [...selectedFiles[fieldName]];
    newFiles.splice(fileIndex, 1);
    setSelectedFiles({ ...selectedFiles, [fieldName]: newFiles });
  }
  return (
    <form
      onSubmit={(e) => {
        e.preventDefault();
        const data: any = {};
        const form = e.target as HTMLFormElement;
        const elements = form.elements;
        for (let i = 0; i < elements.length; i++) {
          const field = elements[i] as HTMLInputElement;
          if (!field.name) continue;
          // if (field.type === "submit") continue;
          if (field.type === "checkbox") {
            data[field.name] = field.checked;
            continue;
          }
          if (field.type === "number") {
            data[field.name] =
              field.value || field.value === "0" ? field.value : undefined;
            continue;
          }
          data[field.name] = field.value;
        }
        setErrors({});
        validationSchema
          .validate(data, { abortEarly: false })
          .then(function () {
            // console.log(data);
            onSubmit({ ...data, ...selectedFiles });
            // form.reset();
          })
          .catch(function (err: any) {
            const errors = GetErrorObject(err);
            // console.log(errors);
            setErrors(errors);
          });
      }}
    >
      <Row>
        {props.fields.map(({ grid, ...field }) => {
          if (
            field.type === FieldTypes.TEXT ||
            field.type === FieldTypes.TEXTAREA ||
            field.type === FieldTypes.NUMBER
          ) {
            return (
              <InputWrapper key={field.id} {...grid}>
                <FormInput
                  {...field}
                  defaultValue={defaultValues[field.name]}
                  isInvalid={!!(errors && errors[field.name])}
                />
                <ErrorMessage {...{ errors, field }} />
              </InputWrapper>
            );
          }
          if (field.type === FieldTypes.CHECKBOX) {
            return (
              <InputWrapper key={field.id} {...grid}>
                <ToggleSwitch
                  {...field}
                  defaultValue={defaultValues[field.name]}
                  style={{ alignSelf: "flex-end", marginBottom: "6px" }}
                />
              </InputWrapper>
            );
          }
          if (field.type === FieldTypes.SELECT) {
            return (
              <InputWrapper key={field.id} {...grid}>
                <AppSelect
                  {...field}
                  defaultValue={defaultValues[field.name]}
                  options={props.masterData[field.optionsName ?? "__"]}
                  isInvalid={!!(errors && errors[field.name])}
                />
                <ErrorMessage {...{ errors, field }} />
              </InputWrapper>
            );
          }
          if (field.type === FieldTypes.IMAGES) {
            return (
              <InputWrapper key={field.id} {...grid}>
                {/* <input
                  type="file"
                  onChange={(x) => handleFilesSelected(x.target.files)}
                /> */}
                <FileUploader
                  onFilesRemoved={(i) => handleFileRemoved(i, field.name)}
                  onFilesSelected={(files) =>
                    handleFilesSelected(files, field.name)
                  }
                  selectedFiles={selectedFiles[field.name]}
                />
                <ErrorMessage {...{ errors, field }} />
              </InputWrapper>
            );
          }
        })}
      </Row>

      <div className="mt-3 d-grid" style={{ justifyContent: "flex-end" }}>
        <Button type="submit" disabled={false}>
          {t("Submit")}
        </Button>
      </div>
    </form>
  );
}

function ErrorMessage({ errors, field }: any) {
  return (
    <>
      {errors && errors[field.name] ? (
        <Form.Control.Feedback type="invalid" className="d-block">
          {errors[field.name]}
        </Form.Control.Feedback>
      ) : null}
    </>
  );
}

function InputWrapper(props: any) {
  return (
    <Col md={props.md || 6} className="mt-3 d-grid">
      {props.children}
    </Col>
  );
}

export function GetErrorObject(err: any) {
  const errors: any = {};
  err.inner.forEach((e: any) => {
    errors[e.path] = e.message;
  });
  return errors;
}

export enum FieldTypes {
  TEXT = "text",
  TEXTAREA = "textarea",
  NUMBER = "number",
  CHECKBOX = "checkbox",
  SELECT = "select",
  IMAGES = "images",
}

export type FieldType = {
  id: string;
  type: FieldTypes;
  name: string;
  label: string;
  placeholder?: string;
  defaultValue?: any;
  style?: CSSProperties;
  grid?: any;
  options?: { value: string; label: string }[];
  optionsName?: string;
  isInvalid?: boolean;
};

type FormPropsType = {
  fields: FieldType[];
  onSubmit: (data: any) => void;
  defaultValues?: any;
  masterData?: any;
  validationSchema?: any;
  files?: any;
};
