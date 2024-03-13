export default function useConfirmationBox__OLD() {
  return function (msg: string, cb: () => void) {
    if (window.confirm(msg)) cb();
  };
}
