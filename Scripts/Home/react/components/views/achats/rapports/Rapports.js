import React from "react";
import { useTitle } from "../../../providers/TitleProvider";
import AchatsFA from "./AchatsFA";

const Rapports = () => {
  const { setTitle } = useTitle();

  React.useEffect(() => {
    setTitle("Rapports");
  }, []);

  return (
    <>
      <AchatsFA />
    </>
  );
};

export default Rapports;
