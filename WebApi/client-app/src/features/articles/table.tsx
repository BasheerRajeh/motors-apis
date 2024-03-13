import { Row, Col, Card } from "react-bootstrap";
// components
import TableV8, { defaultPagination } from "../../components/table-v8";
import { useEffect, useState } from "react";
import { ColumnDef, PaginationState } from "@tanstack/react-table";
import AppDropdown from "../../components/dropdown";
import { useNavigate } from "react-router-dom";
import { useTable } from "react-table";
import useFetch from "../../hooks/use-fetch";
import { toLocalDateTime } from "../../utils/dates";

export default function ViewArticles() {
  const [articles, setArticles] = useState<any>({});
  const [pagination, setPagination] =
    useState<PaginationState>(defaultPagination);
  const { get } = useFetch();
  useEffect(() => {
    (async function () {
      var temp = await get("/articles", {
        query: { PageSize: pagination.pageSize, Page: pagination.pageIndex },
      });
      setArticles(temp || {});
    })();
  }, [pagination.pageSize, pagination.pageIndex]);

  return (
    <TableV8
      columns={columns}
      data={articles.Data || []}
      pageCount={articles.PageCount || 0}
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
          onClick: () => navigate(`/articles/form/${row.Id}`),
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
    header: "Sub Title",
    accessorKey: "SubTitle",
  },
  {
    header: "Location",
    accessorKey: "Location",
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
