import { ButtonGroup, Dropdown } from "react-bootstrap";
import FeatherIcons from "feather-icons-react";
import { PropsWithChildren } from "react";

export default function AppDropdown(
  props: PropsWithChildren<DropdownPropsType>
) {
  return (
    <Dropdown as={ButtonGroup} className="mt-2 me-1">
      <Dropdown.Toggle className="cursor-pointer" variant={"secondary"}>
        {props.title}
        <i className="icon">
          <span>
            <FeatherIcons icon="menu"></FeatherIcons>
          </span>
        </i>
      </Dropdown.Toggle>
      <Dropdown.Menu>
        {props.options.map((option, i) => (
          <Dropdown.Item key={i} onClick={option.onClick}>
            {option.label}
          </Dropdown.Item>
        ))}
      </Dropdown.Menu>
    </Dropdown>
  );
}

type DropdownOptionType = {
  label: string;
  onClick: () => void;
};
type DropdownPropsType = {
  title: string;
  options: DropdownOptionType[];
};
