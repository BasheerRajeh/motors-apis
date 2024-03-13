import { Row, Col, Card } from "react-bootstrap";
// components
import { useEffect, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import TableV8, { defaultPagination } from "../../components/table-v8";
import useFetch from "../../hooks/use-fetch";

export default function ViewUsers() {
  const [users, setUsers] = useState<any>({});
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(() => {
    (async function () {
      var temp = await get("/users", {
        query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
      });
      setUsers(temp || {});
    })();
  }, [pagination.pageSize, pagination.pageIndex]);

  return (
    <TableV8
      columns={columns}
      data={users.Data || []}
      pageCount={users.PageCount || 0}
      {...{ pagination, setPagination }}
    />
  );
}

const columns: ColumnDef<any>[] = [
  {
    header: "ID",
    accessorKey: "Id",
  },
  {
    header: "Full Name",
    accessorKey: "FirstName",
    cell: ({ row }) => {
      const { original: data } = row;
      return (
        <>
          {data.FirstName} {data.LastName}
        </>
      );
    },
  },
  {
    header: "Contact",
    accessorKey: "Contact",
  },
  {
    header: "Email",
    accessorKey: "Email",
  },
  {
    header: "Update Date",
    accessorKey: "UpdatedDate",
  },
];
