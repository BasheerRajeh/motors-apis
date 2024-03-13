import TableV8, { defaultPagination } from "../../components/table-v8";
import { useEffect, useRef, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import AppDropdown from "../../components/dropdown";
import { useNavigate } from "react-router-dom";
import useFetch from "../../hooks/use-fetch";
import { toLocalDateTime } from "../../utils/dates";

export default function ViewServices() {
  const inProgress = useRef<boolean>(false);
  const [services, setServices] = useState<any>({});
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(() => {
    (async function () {
      var temp = await get("/services", {
        query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
      });
      setServices(temp || {});
    })();
  }, [pagination.pageSize, pagination.pageIndex]);

  return (
    <TableV8
      columns={columns}
      data={services.Data || []}
      pageCount={services.PageCount || 0}
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
          onClick: () => navigate(`/services/form/${row.Id}`),
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
    header: "Title",
    accessorKey: "Title",
  },
  {
    header: "Description",
    accessorKey: "Description",
  },
  {
    header: "CreatedDate",
    accessorKey: "CreatedDate",
    cell: ({ row }) => <>{toLocalDateTime(row.original.CreatedDate)}</>,
  },
  {
    header: "UpdatedDate",
    accessorKey: "UpdatedDate",
    cell: ({ row }) => <>{toLocalDateTime(row.original.UpdatedDate)}</>,
  },
  {
    header: "Action",
    // accessorKey: "Id",
    cell: ({ row }) => <RowActions row={row.original} />,
  },
];
