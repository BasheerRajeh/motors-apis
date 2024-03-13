import { Row, Col, Card } from "react-bootstrap";
// components
import TableV8, { defaultPagination } from "../../components/table-v8";
import { useEffect, useRef, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import AppDropdown from "../../components/dropdown";
import { useNavigate } from "react-router-dom";
import { useTable } from "react-table";
import useFetch from "../../hooks/use-fetch";

export default function ViewCars() {
  const inProgress = useRef<boolean>(false);
  const [cars, setCars] = useState<any>({});
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(() => {
    (async function () {
      var temp = await get("/cars", {
        query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
      });
      setCars(temp || {});
    })();
  }, [pagination.pageSize, pagination.pageIndex]);

  return (
    <TableV8
      columns={columns}
      data={cars.Data || []}
      pageCount={cars.PageCount || 0}
      {...{ pagination, setPagination }}
    />
  );
}

function RowActions({ row }: any) {
  const navigate = useNavigate();

  return (
    <AppDropdown
      title=""
      options={[
        {
          label: "Edit",
          onClick: () => navigate(`/cars/form/${row.Id}`),
        },
      ]}
    />
  );
}

const columns: ColumnDef<any>[] = [
  {
    header: "ID",
    accessorKey: "Id",
  },
  {
    header: "Brand",
    accessorKey: "BrandName",
  },
  {
    header: "Name",
    accessorKey: "Name",
  },
  {
    header: "Battery Power",
    accessorKey: "BatteryPower",
  },
  {
    header: "Top Speed",
    accessorKey: "TopSpeed",
  },
  {
    header: "Efficiency",
    accessorKey: "EfficiencyVal",
  },
  {
    header: "Price",
    accessorKey: "Price",
  },
  {
    header: "Action",
    // accessorKey: "Id",
    cell: ({ row }) => <RowActions row={row.original} />,
  },
];
