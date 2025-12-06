const logObject = (obj: Record<string, any>) => {
  console.group("Object Log:");
  for (const [key, value] of Object.entries(obj)) {
    console.log(`${key}: `, value);
  }
  console.groupEnd();
};

export default logObject;
