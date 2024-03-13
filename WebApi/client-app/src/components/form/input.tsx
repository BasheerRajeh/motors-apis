import { PropsWithChildren, useMemo, useState } from "react";
import { FieldType } from "./form";
import { Col, Form, Row } from "react-bootstrap";
import Select from "react-select";

export default function FormInput(props: PropsWithChildren<FieldType>) {
  const numProps = useMemo(
    () =>
      props.type === "number"
        ? {
            onWheel: (e: any) => {
              const target = e.target as any;
              target.blur && target.blur();
            },
            step: "0.01",
          }
        : undefined,
    [props.type]
  );
  return (
    <Form.Group>
      <Form.Label htmlFor={props.id}>{props.label}</Form.Label>
      <Form.Control
        isInvalid={props.isInvalid || false}
        defaultValue={props.defaultValue}
        type={props.type}
        as={props.type === "textarea" ? "textarea" : undefined}
        id={props.id}
        name={props.name}
        placeholder={props.name}
        {...numProps}
      />
    </Form.Group>
  );
}

export function AppSelect(props: PropsWithChildren<FieldType>) {
  const defVal = useMemo(
    () => props?.options?.find((x) => x.value === props.defaultValue),
    [props.defaultValue]
  );
  return (
    <Form.Group>
      <Form.Label htmlFor={props.id}>{props.label}</Form.Label>
      <Select
        id={props.id}
        className="react-select react-select-container"
        classNamePrefix="react-select"
        options={props.options}
        name={props.name}
        defaultValue={defVal}
      ></Select>
    </Form.Group>
  );
}
export function ToggleSwitch(props: PropsWithChildren<FieldType>) {
  //   const [checked, setChecked] = useState(props.defaultValue || false);
  return (
    <Form.Check
      defaultChecked={props.defaultValue}
      type="switch"
      id={props.id}
      label={props.label}
      name={props.name}
      style={{ ...props.style }}
    />
  );
}
